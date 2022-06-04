using Microsoft.AspNetCore.Components.Authorization;

namespace StoreAppUI.Helpers
{
    public static class AuthenticationStateProviderExt
    {
        public async static Task<UserModel> GetUserModelFromAuth(
            this AuthenticationStateProvider authProvider, IUserData userData)
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            return await userData.GetUser(
                authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value
            );
        }
    }
}
