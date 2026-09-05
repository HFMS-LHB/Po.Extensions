namespace Po.Navigation.Core.Interfaces;

public interface INavigationAware
{
    void OnNavigatedTo(NavigationContext context);

    void OnNavigatedFrom(NavigationContext context);

    bool IsNavigationTarget(NavigationContext context);
}
