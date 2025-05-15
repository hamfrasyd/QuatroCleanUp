using System.Text.RegularExpressions;

namespace QuatroCleanUpBackend
{
    public static class ValidateMethodForUser
    {
        public static void ValidateUser(User user)
        {
           if(user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            ValidateUserID(user.UserId);
            ValidateUserName(user.Name);
            ValidateUserEmail(user.Email);
            ValidateUserPassword(user.Password);
        }

        public static void ValidateUserID(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be a positive integer.");
            }
        }
        public static void ValidateUserName(string name)
        {

            if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 50)
            {
                throw new ArgumentException("Name must be between 1 and 50 characters.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-ZæøåÆØÅ\s-]+$"))
            {
                throw new ArgumentException("Name can only contain letters and spaces.");
            }
        }
        public static void ValidateUserEmail(string email)
        {

            if (!Regex.IsMatch(email, @"^.+@.+\..+$"))
            {
                throw new ArgumentException("Not a valid format");
            }
        }

        public static void ValidateUserPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password cannot be empty and must be at least 8 characters long.");
            }

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                else if (char.IsLower(c))
                {
                    hasLowerCase = true;
                }
                else if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
            }

            Regex specialCharRegex = new Regex(@"[!@#$%^&*(),.?""{}|<>]");
            bool hasSpecialChar = specialCharRegex.IsMatch(password);

            if (!hasUpperCase)
            {
                throw new ArgumentException("Password must contain at least one uppercase letter.");
            }
            if (!hasLowerCase)
            {
                throw new ArgumentException("Password must contain at least one lowercase letter.");
            }
            if (!hasDigit)
            {
                throw new ArgumentException("Password must contain at least one digit.");
            }
            if (!hasSpecialChar)
            {
                throw new ArgumentException("Password must contain at least one special character.");
            }

        }
    }
}
