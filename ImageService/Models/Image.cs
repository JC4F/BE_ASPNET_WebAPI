using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Models
{
    public class CloudinaryEnvConfiguration
    {
        public string Name { get; set; }
        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string APIENV { get; set; }
    }

    public class ImageUploadDto
    {
        public IFormFile Image { get; set; }
    }
}
