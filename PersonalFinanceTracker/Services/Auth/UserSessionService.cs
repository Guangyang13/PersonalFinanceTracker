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
        public string Username { get; private set; }
        public string JwtToken { get; private set; }

        public void SetSession(string username, string jwtToken)
        {
            Username = username;
            JwtToken = jwtToken;
        }

        public void ClearSession()
        {
            Username = null;
            JwtToken = null;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(JwtToken);
    }
}
