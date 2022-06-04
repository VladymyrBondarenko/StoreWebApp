using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using StoreAppUI;
using StoreAppUI.Shared;
using StoreAppUI.Helpers;

namespace StoreAppUI.Pages.Products
{
    public partial class Products
    {
        private List<ProductModel> products;

        private UserModel loggedInUser;

        protected override async Task OnInitializedAsync()
        {
            loggedInUser = await authProvider.GetUserModelFromAuth(userData);

            var res = await productData.GetProducts();
            products = res.Where(i => i.IsAvailable).ToList();
        }

        private async Task deleteProduct(ProductModel productModel)
        {
            products.Remove(productModel);
            await productData.DeleteProduct(productModel.ProductId);
        }

        private void openCreateProduct()
        {
            navManager.NavigateTo("/CreateProduct");
        }

        private void openEditProduct(ProductModel product)
        {
            navManager.NavigateTo($"/EditProduct/{product.ProductId}");
        }

        private void openOrderProduct(ProductModel product)
        {
            if (loggedInUser is null)
            {
                navManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
            }
            else
            {
                navManager.NavigateTo($"/OrderProduct/{product.ProductId}");
            }
        }

        private string getImagePath(string imagePath)
        {
            var path = string.Empty;
            if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
            {
                var fileName = Path.GetFileName(imagePath);
                path = Path.Combine(Settings.ProductFileDir, fileName);
            }

            return path;
        }

        private async Task<string> getEditButtonClass()
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var authUserClaims = authState.User.Claims;

            string className;
            var isAdmin = authUserClaims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value == "admin";
            if (isAdmin)
            {
                className = "card-edit-visible";
            }
            else
            {
                className = "card-edit-hidden";
            }
            return className;
        }

        private async Task<string> getDeleteButtonClass()
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var authUserClaims = authState.User.Claims;

            string className;
            var isAdmin = authUserClaims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value == "admin";
            if (isAdmin)
            {
                className = "card-delete-visible";
            }
            else
            {
                className = "card-delete-hidden";
            }
            return className;
        }

        private async Task<string> getAddProdBtnClass()
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var authUserClaims = authState.User.Claims;

            string className;
            var isAdmin = authUserClaims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value == "admin";
            if (isAdmin)
            {
                className = "prod-addbtn-visible";
            }
            else
            {
                className = "prod-addbtn-hidden";
            }
            return className;
        }
    }
}