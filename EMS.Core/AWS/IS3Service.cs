using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EMS.Core.AWS
{
    public interface IS3Service
    {
        Task UploadFileToS3(IFormFile file, string key);
        string GenereatePresignedURL(string key);
    }
}