using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BabyMoo.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
