using CommunityToolkit.Mvvm.ComponentModel;

using DialogHostAvalonia;

using Po.DialogHost.Avalonia.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Po.DialogHost.Avalonia.Dialogs;

/// <summary>
/// Dialog基类
/// </summary>
public abstract class PoDialogBase<TData>() : ObservableValidator, IPoDialogPolicy, IPoDialogSessionAware
{
    private DialogSession? _session;

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

    public void SetSession(DialogSession session)
    {
        _session = session;
    }
}