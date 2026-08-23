using Po.DialogHost.Avalonia.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.DialogHost.Avalonia.Dialogs;

/// <summary>
/// 表单Dialog
/// </summary>
/// <param name="poDialogService"></param>
public abstract class FormDialogBase<TData>(IPoDialogService poDialogService) : PoDialogBase<TData>(poDialogService)
{
    private readonly IPoDialogService _poDialogService = poDialogService;

    protected virtual bool Validate() => true;

    protected void Confirm()
    {
        if (!Validate()) return;
        IsConfirmed = true;
        _poDialogService.Close();
    }
}
