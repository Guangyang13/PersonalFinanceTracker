using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Models.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDto()
        {
            
        }
        public UserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
