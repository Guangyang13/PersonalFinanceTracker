using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Interfaces.Auth
{
    public interface IUserSessionService
    {
        void SetSession(string username, string token);
        void ClearSession();

        string Username { get; }
        string JwtToken { get; }

        bool IsLoggedIn { get; }
    }
}
