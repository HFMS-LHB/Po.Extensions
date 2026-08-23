using Po.DialogHost.Avalonia.Interfaces;
using Po.MVVM.Core.DependencyInjection;

namespace Po.DialogHost.Avalonia.Dialogs;

/// <summary>
/// 表单Dialog
/// </summary>
public abstract class FormDialogBase<TData> : PoDialogBase<TData>
{
    private readonly IPoDialogService _poDialogService;

    protected FormDialogBase()
    {
        _poDialogService = PoContainer.GetRequiredService<IPoDialogService>();
    }

    protected virtual bool Validate() => true;

    protected void Confirm()
    {
        if (!Validate()) return;
        IsConfirmed = true;
        _poDialogService.Close();
    }
}
