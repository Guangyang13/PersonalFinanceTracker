using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> IsUserRegisteredAsync(string username);
        Task<bool> RegisterAsync(string username, string password);
    }
}
