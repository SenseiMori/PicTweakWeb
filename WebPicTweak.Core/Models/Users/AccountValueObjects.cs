using WebPicTweak.Core.Models.SeedWork;

namespace WebPicTweak.Core.Models.Users
{
    public sealed class NickName : BaseValueObject
    {
        public NickName() { }
        public string Value { get; private set; } = string.Empty;
        public NickName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Nickname cannot be empty.");
            }
            if (value.Length < 3 || value.Length > 20)
            {
                throw new ArgumentException("Nickname must be between 3 and 20 characters long.");
            }
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }


    }
    public sealed class Email : BaseValueObject
    {
        public Email() { }

        public string Value { get; private set; } = string.Empty;

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email cannot be empty.");
            }
            if (value.Length < 3 || value.Length > 20)
            {
                throw new ArgumentException("Nickname must be between 3 and 20 characters long.");
            }
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
    public sealed class Password : BaseValueObject
    {
        public Password() { }
        public string Value { get; private set; } = string.Empty;
        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("null");
            }
            //if (value.Length <= 6 || value.Length > 20)
            //{
            //    throw new ArgumentException("length password");
            //}
            //if (CheckPassword(value))
            //{
            //    throw new ArgumentException("not corrected");
            //}
            Value = value;
        }

        //private static bool CheckPassword (string value)
        //{
        //    return value.Any(char.IsUpper) &&
        //        value.Any(char.IsLower) &&
        //        value.Any(char.IsDigit);
        //}

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
    public sealed class PasswordHash : BaseValueObject
    {
        public PasswordHash() { }
        public string Value { get; private set; } = string.Empty;
        public PasswordHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("PasswordHash не может быть пустым.");
            }
            if (value.Length < 3 || value.Length > 200)
            {
                throw new ArgumentException("PasswordHash должен быть от 3 до 200 символов.");
            }
            Value = value;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
