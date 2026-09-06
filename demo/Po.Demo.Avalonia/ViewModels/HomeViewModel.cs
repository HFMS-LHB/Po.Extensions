using CommunityToolkit.Mvvm.Input;

using Po.DialogHost.Core.Interfaces;
using Po.MVVM.Core.DependencyInjection;
using Po.Navigation.Core;

using System.Threading.Tasks;

namespace Po.Demo.Avalonia.ViewModels;

public partial class HomeViewModel : NavigationViewModelBase
{
    private readonly IPoDialogService _dialogService;

    public HomeViewModel(IPoDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [RelayCommand]
    private async Task ShowTestDialogAsync()
    {
        // var result = await _dialogService.ShowAsync<TestDialogViewModel, string>("test data");


        var vm = PoContainer.GetRequiredService<TestDialogViewModel>();
        _ = _dialogService.ShowAsync(vm);
        await vm.RunAsync(async (token) =>
        {
            await Task.Delay(1000 * 5, token);
        });

        // var result = await _dialogService.ShowAsync<TestDialogViewModel, string>("test data");
        // 这里可以处理 result
    }
}
