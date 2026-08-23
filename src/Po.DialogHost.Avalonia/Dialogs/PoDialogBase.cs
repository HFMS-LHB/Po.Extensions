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
/// <param name="poDialogService"></param>
public abstract class PoDialogBase<TData>(IPoDialogService poDialogService) : ObservableValidator, IPoDialogPolicy
{
    private DialogSession? _session;

    public TData? Data { get; protected set; }
    public bool IsWorkCompleted { get; protected set; } = true;
    public virtual bool CanClose => IsWorkCompleted;
    public bool IsConfirmed { get; protected set; } = false;
    public virtual bool CloseOnClickAway => false;

    public event Action? DialogOpened;
    public event Action? DialogClosed;

    protected internal virtual void Initialize(TData data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        OnDataInitialized();
    }

    protected virtual void OnDataInitialized() { }

    internal void SetSession(DialogSession session)
    {
        _session = session;
    }

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

}