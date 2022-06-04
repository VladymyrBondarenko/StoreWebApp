using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace StoreAppUI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
            builder.Services.AddMemoryCache();
            builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
            builder.Services.AddAuthorization(o =>
                o.AddPolicy("admin", p =>
                {
                    p.RequireClaim("jobTitle", "admin");
                }));

            builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            builder.Services.AddSingleton<IUserData, UserData>();
            builder.Services.AddSingleton<IProductData, ProductData>();
            builder.Services.AddSingleton<IOrderData, OrderData>();
            builder.Services.AddSingleton<IPictureFileManager, PictureFileManager>();
        }
    }
}
