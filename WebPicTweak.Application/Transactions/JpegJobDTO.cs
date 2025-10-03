using Microsoft.AspNetCore.Http;
using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Application.Transactions
{
    public class JpegJobDTO
    {
        public Guid TransactionId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public ModifierOptions Options { get; set; }
        public IFormFile File { get; set; }
    }
}
