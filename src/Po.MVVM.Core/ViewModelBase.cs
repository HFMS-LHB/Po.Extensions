using CommunityToolkit.Mvvm.ComponentModel;

using Po.MVVM.Core.Interfaces;

namespace Po.MVVM.Core;

public abstract class ViewModelBase : ObservableValidator, IDestructible
{
    protected ViewModelBase()
    {

    }
    public virtual void Destroy()
    {

    }
}
