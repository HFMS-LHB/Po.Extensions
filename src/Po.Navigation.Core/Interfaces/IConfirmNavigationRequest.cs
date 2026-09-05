namespace Po.Navigation.Core.Interfaces;

public interface IConfirmNavigationRequest
{
    void ConfirmNavigationRequest(
        NavigationContext context,
        Action<bool> callback);
}
