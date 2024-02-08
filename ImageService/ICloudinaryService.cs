using ImageService.Models;

namespace ImageService
{
    public interface ICloudinaryService
    {
        Task<string> UploadPicture(ImageUploadDto model);
    }
}