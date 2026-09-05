using System;

namespace Po.DialogHost.Core.Interfaces;

public interface IPoDialogPolicy
{
    bool IsWorkCompleted { get; }

    bool CanClose { get; }

    bool IsConfirmed { get; }

    bool CloseOnClickAway { get; }

    event Action? DialogOpened;

    event Action? DialogClosed;

    void OnDialogOpened();

    void OnDialogClosed();

    void RequestCancel();
}
