using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Core.Interfaces
{
    public interface INavigationLifecycleBehavior
    {
        void OnNavigatedTo(object viewModel, NavigationContext context);

        void OnNavigatedFrom(object viewModel, NavigationContext context);

        void OnDestroy(object viewModel);
    }
}
