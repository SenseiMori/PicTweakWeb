using WebPicTweak.Core.Models.Log;

namespace WebPicTweak.Core.Models.Users
{
    public abstract class BaseAccount
    {
        public Guid Id { get; protected set; }
        public DateTime RegistrationDate { get; protected set; } = DateTime.UtcNow;
        public UserLog UserLog { get; protected set; }
        protected BaseAccount(Guid id)
        {
            Id = id;
            RegistrationDate = DateTime.UtcNow;
            UserLog = UserLog.Create(id);
        }
        protected BaseAccount() { }
    }
    public class Account : BaseAccount
    {
        public NickName NickName { get; private set; }
        public Email Email { get; private set; }
        public PasswordHash PasswordHash { get; private set; }

        private Account() : base() { }

        private Account(Guid id, NickName nickName, Email email, PasswordHash passwordHash) : base(id)
        {
            NickName = nickName;
            Email = email;
            PasswordHash = passwordHash;
        }
        public static Account Create(NickName nickName, Email email, PasswordHash passwordHash)
        {
            return new Account(Guid.NewGuid(), nickName, email, passwordHash);
        }
        public static Account Get(Guid Id)
        {
            return new Account
            {
                Id = Id
            };
        }
    }
    public class GuestAccount : BaseAccount
    {
        public DateTime EntryTime { get; private set; }
        private GuestAccount() : base() { }
        private GuestAccount(Guid id) : base(id)
        {
            EntryTime = DateTime.UtcNow;
        }
        public static GuestAccount CreateGuest()
        {
            return new GuestAccount(Guid.NewGuid());
        }
    }
}
