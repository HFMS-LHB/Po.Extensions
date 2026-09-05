using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.Lifecycle;

/// <summary>
/// 默认的一次性异步信号实现。
/// </summary>
public class AsyncSignal : IAsyncSignal
{
    private readonly TaskCompletionSource _tcs =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    /// <inheritdoc />
    public bool IsSet => _tcs.Task.IsCompleted;

    /// <inheritdoc />
    public Task WaitAsync(CancellationToken cancellationToken = default)
    {
        return _tcs.Task.WaitAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Set()
    {
        _tcs.TrySetResult();
    }
}
