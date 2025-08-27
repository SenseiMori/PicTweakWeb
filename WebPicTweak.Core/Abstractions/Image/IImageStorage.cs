using Microsoft.AspNetCore.Http;
using WebPicTweak.Core.Models.Image;


// убрать, так как не взаимодействует с бд

namespace WebPicTweak.Core.Abstractions.Image
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(IFormFile file);
        Task<bool> DeleteAsync(string path);
        Task<Jpeg?> GetByIdAsync(ImageId id);


    }
}
