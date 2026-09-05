using Po.DialogHost.Core.Interfaces;

using System;

namespace Po.DialogHost.DialogHostAvalonia;

public class PoDialogHost : global::DialogHostAvalonia.DialogHost
{
    protected override Type StyleKeyOverride => typeof(global::DialogHostAvalonia.DialogHost);

    public PoDialogHost()
    {
        DialogOpened += OnDialogOpened;
        DialogClosing += OnDialogClosing;
    }

    private void OnDialogOpened(object? sender, global::DialogHostAvalonia.DialogOpenedEventArgs e)
    {
        if (e.Session.Content is IPoDialogSessionAware aware)
        {
            aware.SetSession(new DialogHostAvaloniaSession(e.Session));
        }

        if (e.Session.Content is IPoDialogPolicy policy)
        {
            CloseOnClickAway = policy.CloseOnClickAway;

            policy.OnDialogOpened();
        }
        else
        {
            CloseOnClickAway = true;
        }
    }

    private void OnDialogClosing(object? sender, global::DialogHostAvalonia.DialogClosingEventArgs e)
    {
        if (e.Session.Content is not IPoDialogPolicy policy) return;

        if (!policy.CanClose)
        {
            e.Cancel();

            policy.RequestCancel();

            return;
        }


        policy.OnDialogClosed();
    }
}
