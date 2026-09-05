using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.SingleInstance;

/// <summary>
/// Linux 平台单实例服务实现
/// </summary>
internal sealed class LinuxSingleInstanceService : ISingleInstanceService
{
    private readonly string _socketPath;
    private readonly Action? _onActivate;
    private Socket? _serverSocket;
    private CancellationTokenSource? _cts;
    private bool _isOwner;

    public LinuxSingleInstanceService(SingleInstanceOptions options)
    {
        // 建议放在 /tmp 下，UOS 对此路径有良好的读写支持
        _socketPath = Path.Combine(Path.GetTempPath(), $"{options.MutexName}.sock");
        _onActivate = options.OnActivate;
    }

    public bool TryAcquireOwnership()
    {
        try
        {
            // 1. 检查 Socket 文件是否存在
            if (File.Exists(_socketPath))
            {
                using var client = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                try
                {
                    // 尝试连接，如果能连上，说明已有主实例在运行
                    client.Connect(new UnixDomainSocketEndPoint(_socketPath));
                    return false;
                }
                catch (SocketException)
                {
                    // 连不上说明是上次崩溃残留的无效文件，清理掉
                    File.Delete(_socketPath);
                }
            }

            // 2. 尝试成为主实例（建立监听）
            _serverSocket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            _serverSocket.Bind(new UnixDomainSocketEndPoint(_socketPath));
            _serverSocket.Listen(5);

            _isOwner = true;
            _cts = new CancellationTokenSource();

            // 在后台监听来自其他实例的消息
            Task.Run(() => ListenForActivation(_cts.Token));

            return true;
        }
        catch (Exception)
        {
            // 如果因为权限或其他原因失败，保守起见返回 true 让程序先跑起来
            return true;
        }
    }

    public void ActivateExistingInstance()
    {
        // 当 TryAcquireOwnership 返回 false 时，本方法被调用
        try
        {
            using var client = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            client.Connect(new UnixDomainSocketEndPoint(_socketPath));

            // 发送激活指令
            byte[] data = Encoding.UTF8.GetBytes("ACTIVATE");
            client.Send(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"无法激活现有实例: {ex.Message}");
        }
    }

    private async Task ListenForActivation(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _serverSocket != null)
        {
            try
            {
                using var handler = await _serverSocket.AcceptAsync();
                byte[] buffer = new byte[1024];
                int received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                string message = Encoding.UTF8.GetString(buffer, 0, received);

                if (message == "ACTIVATE" && _onActivate != null)
                {
                    // 触发回调逻辑
                    _onActivate.Invoke();
                }
            }
            catch
            {
                if (token.IsCancellationRequested) break;
            }
        }
    }

    public void ReleaseOwnership()
    {
        _isOwner = false;
        Dispose();
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        if (_serverSocket != null)
        {
            _serverSocket.Close();
            _serverSocket.Dispose();
            _serverSocket = null;
        }

        if (File.Exists(_socketPath))
        {
            try { File.Delete(_socketPath); } catch { /* ignore */ }
        }
    }
}
