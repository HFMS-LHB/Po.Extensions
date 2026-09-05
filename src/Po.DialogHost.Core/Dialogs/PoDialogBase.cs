using CommunityToolkit.Mvvm.ComponentModel;

using Po.DialogHost.Core.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Po.DialogHost.Core.Dialogs;

/// <summary>
/// Dialog基类
/// </summary>
public abstract class PoDialogBase<TData>() : ObservableValidator, IPoDialogPolicy, IPoDialogSessionAware
{
    private IPoDialogSession? _session;

    public TData? Data { get; protected set; }
    public bool IsWorkCompleted { get; protected set; } = true;
    public virtual bool CanClose => IsWorkCompleted;
    public bool IsConfirmed { get; protected set; } = false;
    public virtual bool CloseOnClickAway => false;

    public event Action? DialogOpened;
    public event Action? DialogClosed;

    protected internal void Initialize(TData data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        OnDataChanged(data);
    }

    public virtual void OnDataChanged(TData data) { }

    public virtual void RequestCancel()
    {
        if (IsWorkCompleted)
        {
            Close();
        }
    }

    public virtual void OnDialogOpened()
    {
        IsConfirmed = false;
        DialogOpened?.Invoke();
    }

    public virtual void OnDialogClosed()
    {
        DialogClosed?.Invoke();
    }

    protected void Close(object? parameter = null)
    {
        _session?.Close(parameter);
    }

    public void SetSession(IPoDialogSession session)
    {
        _session = session;
    }
}
