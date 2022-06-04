using Microsoft.AspNetCore.Components.Forms;

namespace StoreAppUI.Tools
{
    public class PictureFileManager : IPictureFileManager
    {
        public async Task ProccessFile(IBrowserFile imageFile, string targerPath)
        {
            var resizedImage = await imageFile.RequestImageFileAsync("image/jpg", 250, 250);
            var imageStream = resizedImage.OpenReadStream();

            try
            {
                using (var fileStream = new FileStream(targerPath, FileMode.Create))
                {
                    await imageStream.CopyToAsync(fileStream);
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}