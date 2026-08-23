using DialogHostAvalonia;

using Po.DialogHost.Avalonia.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.DialogHost.Avalonia
{
    public class PoDialogHost : DialogHostAvalonia.DialogHost
    {
        protected override Type StyleKeyOverride => typeof(DialogHostAvalonia.DialogHost);

        public PoDialogHost()
        {
            DialogOpened += OnDialogOpened;
            DialogClosing += OnDialogClosing;
        }

        private void OnDialogOpened(object? sender, DialogOpenedEventArgs e)
        {
            if (e.Session.Content is IPoDialogSessionAware aware)
            {
                aware.SetSession(e.Session);
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

        private void OnDialogClosing(object? sender, DialogClosingEventArgs e)
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
}
