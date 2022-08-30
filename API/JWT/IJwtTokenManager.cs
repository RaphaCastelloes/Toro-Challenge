using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.JWT
{
    public interface IJwtTokenManager
    {
        // Authentication method
        string Authenticate(string userName, string password);
    }
}