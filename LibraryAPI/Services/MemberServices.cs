using BCrypt.Net;

namespace LibraryAPI.Services
{
    public class MemberServices
    {
        public string HashPassword(string password, out string salt)
        {
            salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

    }
}