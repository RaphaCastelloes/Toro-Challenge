using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    // Used to transport the user credentials throw the API
    public class UserCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}