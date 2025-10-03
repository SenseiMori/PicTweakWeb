using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Application.Abstractions
{
    public interface ISessionStore
    {
        Guid Create(TimeSpan ttl, Guid userId);
        void SetPath(Guid sessionId, string path);
        bool Remove(Guid sessionId);
        ModifierSessionDTO GetSession(Guid sessionId);
    }
}
