using WebPicTweak.Core.Models.Users;

namespace WebPicTweak.Application.Services.UserServices.Registration
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccountRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        public AccountService(IPasswordHasher passwordHasher, IAccountRepository userRepository, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }
        public async Task<Account> RegisterAccountAsync(string userName, string email, string password, CancellationToken ct)
        {
            var isExistingUser = await _userRepository.GetByEmailAsync(email);
            if (isExistingUser != null)
            {
                throw new InvalidOperationException("Почта уже зарегистрирована.");
            }
            var nickNameVo = new NickName(userName);
            var emailVo = new Email(email);
            var hashedPassword = _passwordHasher.Generate(password);
            var passVo = new PasswordHash(hashedPassword);

            var account = Account.Create(nickNameVo, emailVo, passVo);

            await _userRepository.SaveAsync(account);
            return account;
        }
        public async Task<GuestAccount> CreateGuestAccountAsync(CancellationToken ct)
        {
            var guestAccount = GuestAccount.CreateGuest();
            await _userRepository.SaveAsync(guestAccount);
            return guestAccount;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user is not Account account)
            {
                throw new UnauthorizedAccessException("Неправильная почта или пароль.");
            }
            if (user == null || !_passwordHasher.Verify(account.PasswordHash.Value, password))
            {
                throw new UnauthorizedAccessException("Неправильная почта или пароль.");
            }
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }
        public async Task<BaseAccount> GetAccountByIdAsync(Guid id)
        {
            var account = await _userRepository.GetAccountByIdAsync(id);
            return account;
        }
        public async Task<GuestAccount> GetGuestAccountByIdAsync(Guid id, CancellationToken ct)
        {
            var guestAccount = await _userRepository.GetGuestAccountByIdAsync(id);
            return guestAccount;
        }
    }
}
