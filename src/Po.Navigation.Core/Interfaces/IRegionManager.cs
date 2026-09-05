namespace Po.Navigation.Core.Interfaces;

public interface IRegionManager
{
    /// <summary>
    /// 页面导航
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="view"></param>
    void Navigate(string regionName, object view);

    /// <summary>
    /// 页面导航
    /// </summary>
    Task RequestNavigate(string regionName, string key, NavigationParameters? parameters = null);
}
