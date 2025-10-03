using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WebPicTweak.Application.Abstractions;
using WebPicTweak.Application.Transactions;

namespace WebPicTweak.Infrastructure.Services
{
    public class SessionStore : ISessionStore
    {
        private readonly ConcurrentDictionary<Guid, ModifierSessionDTO> _store = new();
        public Guid Create(TimeSpan ttl, Guid userGuid)
        {
            var sessionId = Guid.NewGuid();
            var session = new ModifierSessionDTO
            {
                SessionId = sessionId,
                UserId = userGuid,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(ttl)
            };
            _store[sessionId] = session;
            return sessionId;
        }
        public bool Remove(Guid sessionId)
        {
            return _store.TryRemove(sessionId, out _);
        }
        public void SetPath(Guid sessionId, string path)
        {
            if (_store.TryGetValue(sessionId, out var session))
            {
                session.PathToZip = path;
                session.ExpiresAt = DateTime.UtcNow.AddMinutes(10); 
            }
        }
        public ModifierSessionDTO? GetSession(Guid sessionId)
        {
            _store.TryGetValue(sessionId, out var session);
            if (session.ExpiresAt > DateTime.UtcNow)
            {
                return session;
            }
            _store.TryRemove(sessionId, out _);
            return null;
        }
    }
}
