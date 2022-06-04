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

namespace StoreAppUI.Pages.Products
{
    public partial class EditProduct
    {
        [Parameter]
        public string Id { get; set; }

        private ProductModel productModel { get; set; }

        private IBrowserFile productImageFile;
        protected override async Task OnInitializedAsync()
        {
            productModel = await productData.GetProduct(Convert.ToInt32(Id));
        }

        private void closePage()
        {
            navManager.NavigateTo("/Products");
        }

        private async Task editProd()
        {
            if (productImageFile is not null)
            {
                if (File.Exists(productModel.ImagePath))
                {
                    File.Delete(productModel.ImagePath);
                }

                var filePath = $"{appEnvironment.WebRootPath}/{Settings.ProductFileDir}/{productImageFile.Name}";
                await fileManager.ProccessFile(productImageFile, filePath);
                if (!string.IsNullOrEmpty(filePath))
                {
                    productModel.ImagePath = filePath;
                }
            }

            await productData.UpdateProduct(productModel);
            closePage();
        }

        private void memorizeFile(IBrowserFile file)
        {
            productImageFile = file;
        }
    }
}