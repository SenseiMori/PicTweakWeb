namespace WebPicTweak.Core.Models.SeedWork
{
    public interface IBusinessRule
    {
        bool IsBroken();
        string Message { get; }
    }
}
