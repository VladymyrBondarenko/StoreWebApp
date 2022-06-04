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
    public partial class OrderProduct
    {
        [Parameter]
        public string Id { get; set; }

        private ProductModel productModel;

        private UserModel loggedInUser;

        protected override async Task OnInitializedAsync()
        {
            loggedInUser = await authProvider.GetUserModelFromAuth(userData);
            productModel = await productData.GetProduct(Convert.ToInt32(Id));
        }

        private async Task createOrder(ProductModel p)
        {
            if(loggedInUser == null)
            {
                navManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
            }
            else
            {
                var order = new OrderModel
                {
                    Product = p,
                    User = loggedInUser,
                    IsApproved = false
                };

                await orderData.CreateOrder(order);
                navManager.NavigateTo("/products");
            }
        }
    }
}