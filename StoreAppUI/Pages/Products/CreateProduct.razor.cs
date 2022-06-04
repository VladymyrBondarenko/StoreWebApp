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
    public partial class CreateProduct
    {
        private ProductModel product = new();
        private IBrowserFile productImageFile;

        private void closePage()
        {
            navManager.NavigateTo("/Products");
        }

        private async Task createProd()
        {
            if(!string.IsNullOrWhiteSpace(product.ShortDescription) && !string.IsNullOrWhiteSpace(product.LongDescription))
            {
                var p = new ProductModel
                {
                    ShortDescription = product.ShortDescription,
                    LongDescription = product.LongDescription,
                    Price = product.Price
                };
                if (productImageFile is not null)
                {
                    var filePath = Path.Combine(appEnvironment.WebRootPath, Settings.ProductFileDir, productImageFile.Name);
                    await fileManager.ProccessFile(productImageFile, filePath);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        p.ImagePath = filePath;
                    }
                }

                await productData.CreateProduct(p);
            }
 
            product = new();
            closePage();
        }

        private void memorizeFile(IBrowserFile file)
        {
            productImageFile = file;
        }
    }
}