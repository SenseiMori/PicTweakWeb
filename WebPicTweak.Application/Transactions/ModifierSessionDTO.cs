using WebPicTweak.Core.Models.Image;

namespace WebPicTweak.Application.Transactions
{
    public class ModifierSessionDTO
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public string? PathToZip { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public ModifierOptions Options { get; set; }
    }
}
