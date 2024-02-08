using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class CloudinaryService : ICloudinaryService
    {
        private Cloudinary cloudinary { get; set; }

        public CloudinaryService(CloudinaryEnvConfiguration config)
        {
            this.cloudinary = new Cloudinary(new Account{ ApiKey = config.APIKey, ApiSecret = config.APISecret, Cloud = config.Name });
        }
        public async Task<string> UploadPicture(ImageUploadDto model)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                model.Image.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);

            var prefix = @"data:image/png;base64,";
            var imagePath = prefix + base64;
            var uploadParams = new ImageUploadParams()

            {
                File = new FileDescription(imagePath),
                Folder = "EndSars/img"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.Url.ToString();
        }
    }
}
