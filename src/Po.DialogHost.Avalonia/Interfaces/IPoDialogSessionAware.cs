using DialogHostAvalonia;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.DialogHost.Avalonia.Interfaces
{
    public interface IPoDialogSessionAware
    {
        void SetSession(DialogSession session);
    }
}
