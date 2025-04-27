using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Interfaces.Infrastructure
{
    public interface INetworkService
    {
        Task<bool> IsServerReachableAsync();
    }
}
