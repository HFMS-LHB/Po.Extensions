using Po.DialogHost.Core.Dialogs;
using Po.DialogHost.Core.Interfaces;
using Po.MVVM.Core.DependencyInjection;

using System.Threading.Tasks;

namespace Po.DialogHost.Core;

public class PoDialogService(IDialogHostAdapter adapter) : IPoDialogService
{
    public TDialog GetDialogViewModel<TDialog>() where TDialog : class
    {
        return PoContainer.GetRequiredService<TDialog>();
    }

    public Task<object?> ShowAsync(object content, string? hostIdentifier = "Main")
    {
        return adapter.ShowAsync(content, hostIdentifier);
    }

    public Task<object?> ShowAsync<TDialog, TData>(TData data, string? hostIdentifier = "Main") where TDialog : PoDialogBase<TData>
    {
        var vm = PoContainer.GetRequiredService<TDialog>();

        vm.Initialize(data);

        return ShowAsync(vm, hostIdentifier);
    }

    public Task<object?> ShowAsync<TData>(object content, TData data, string? hostIdentifier = "Main")
    {
        if (content is PoDialogBase<TData> vm && vm != null)
        {
            vm.Initialize(data);

            return ShowAsync(vm, hostIdentifier);
        }

        return Task.FromResult<object?>(null);
    }

    public void Close(string? hostIdentifier = "Main", object? parameter = null)
    {
        adapter.Close(hostIdentifier, parameter);
    }

    public bool IsDialogOpen(string? hostIdentifier = "Main")
    {
        return adapter.IsDialogOpen(hostIdentifier);
    }
}
