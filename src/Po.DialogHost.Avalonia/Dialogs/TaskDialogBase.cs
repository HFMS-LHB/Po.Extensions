using Po.DialogHost.Avalonia.Interfaces;
using Po.MVVM.Core.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.DialogHost.Avalonia.Dialogs;

/// <summary>
/// 任务Dialog
/// </summary>
/// <param name="poDialogService"></param>
public abstract class TaskDialogBase<TData> : PoDialogBase<TData>
{
    private readonly IPoDialogService _poDialogService;
    private bool _closeRequested = false;
    private CancellationTokenSource? _cts;

    protected TaskDialogBase()
    {
        _poDialogService = PoContainer.GetRequiredService<IPoDialogService>();
    }

    /// <summary>
    /// 取消
    /// </summary>
    public override void RequestCancel()
    {
        if (IsWorkCompleted)
        {
            _poDialogService.Close();
        }
        else
        {
            _closeRequested = true;
            _cts?.Cancel();
        }
    }

    /// <summary>
    /// 任务完成
    /// </summary>
    protected virtual void OnCompleted()
    {
        _poDialogService.Close();
    }

    /// <summary>
    /// 任务失败
    /// </summary>
    /// <param name="ex"></param>
    protected virtual void OnFaulted(Exception ex) { }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="work"></param>
    /// <param name="onFaulted"></param>
    /// <returns></returns>
    public async Task RunAsync(Func<CancellationToken, Task> work, Func<Exception, Task>? onFaulted = null)
    {
        IsWorkCompleted = false;
        _cts = new CancellationTokenSource();

        try
        {
            await work(_cts.Token);
        }
        catch (Exception ex)
        {
            if (onFaulted != null)
            {
                await onFaulted(ex);
            }
            OnFaulted(ex);
        }
        finally
        {
            IsWorkCompleted = true;
            _cts.Dispose();
            _cts = null;

            if (_closeRequested)
            {
                _closeRequested = false;
                _poDialogService.Close();
            }
            else
            {
                OnCompleted();
            }
        }
    }
}
