using Microsoft.AspNetCore.Components.Forms;

namespace StoreAppUI.Tools
{
    public interface IFileManager
    {
        Task ProccessFile(IBrowserFile imageFile, string targerPath);
    }
}