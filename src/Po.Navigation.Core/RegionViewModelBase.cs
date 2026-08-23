using Microsoft.Extensions.DependencyInjection;

using Po.MVVM.Core;
using Po.MVVM.Core.DependencyInjection;
using Po.Navigation.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Core;

public partial class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest, IRegionMemberLifetime
{
    private IEnumerable<INavigationLifecycleBehavior>? _behaviors;
    public RegionViewModelBase()
    {
        ErrorsChanged += (s, e) => OnPropertyChanged(nameof(CanSubmit));
    }

    public IEnumerable<INavigationLifecycleBehavior> Behaviors
    {
        get
        {
            return _behaviors ??= PoContainer.Provider.GetServices<INavigationLifecycleBehavior>();
        }
    }

    public virtual bool KeepAlive => false;

    public bool CanSubmit => !HasErrors;

    public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
    {
        continuationCallback(true);
    }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
        foreach (var behavior in Behaviors)
        {
            behavior.OnNavigatedFrom(this, navigationContext);
        }
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
        foreach (var behavior in Behaviors)
        {
            behavior.OnNavigatedTo(this, navigationContext);
        }
    }

    protected bool ValidateAll()
    {
        ValidateAllProperties();
        OnPropertyChanged(nameof(CanSubmit));
        return !HasErrors;
    }

    protected void ValidateProperty(string propertyName)
    {
        ValidateProperty(GetType().GetProperty(propertyName)?.GetValue(this), propertyName);
        OnPropertyChanged(nameof(CanSubmit));
    }

    public override void Destroy()
    {
        foreach (var behavior in Behaviors)
        {
            behavior.OnDestroy(this);
        }
    }
}
