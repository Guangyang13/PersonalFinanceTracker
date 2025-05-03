using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceTracker.Interfaces.Auth;

namespace PersonalFinanceTracker.Services.Auth
{
    public class UserSessionService : IUserSessionService
    {
        public string Username { get; private set; } = string.Empty;
        public string JwtToken { get; private set; } = string.Empty;

        public void SetSession(string username, string jwtToken)
        {
            Username = username;
            JwtToken = jwtToken;
        }

        public void ClearSession()
        {
            Username = string.Empty;
            JwtToken = string.Empty;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(JwtToken);
    }
}
