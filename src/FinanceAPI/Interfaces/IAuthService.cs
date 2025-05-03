using System.Security.Cryptography;

namespace FinanceAPI.Interfaces
{
    public interface IAuthService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);

        string GenerateJwtToken(string username);
    }
}
