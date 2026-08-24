using Avalonia.Threading;

using CommunityToolkit.Mvvm.Input;

using Po.MVVM.Core;
using Po.Navigation.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Demo.Avalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        [RelayCommand]
        private async Task RegionLoaded()
        {
            var targetView = "HomeView";

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _regionManager.RequestNavigate("MainRegion", targetView);
            }, DispatcherPriority.Background);
        }
    }
}
