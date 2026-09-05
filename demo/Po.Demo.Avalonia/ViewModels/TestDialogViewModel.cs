using CommunityToolkit.Mvvm.Input;

using Po.DialogHost.Core.Dialogs;

using System.Threading;
using System.Threading.Tasks;

namespace Po.Demo.Avalonia.ViewModels;

public partial class TestDialogViewModel : TaskDialogBase<string>
{
    public string Message { get; } = "Hello from Po.DialogHost.DialogHostAvalonia!";

    [RelayCommand]
    private async Task StartWorkAsync()
    {
        await RunAsync(async token =>
        {
            await Task.Delay(3000, token);
            if (token.IsCancellationRequested)
            {
                return;
            }
        });
    }

    [RelayCommand]
    private void CancelDialog()
    {
        RequestCancel();
    }
}
