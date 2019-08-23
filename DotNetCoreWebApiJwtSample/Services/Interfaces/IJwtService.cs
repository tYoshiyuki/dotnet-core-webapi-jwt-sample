using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateEncodedToken(string userName, IList<string> roles = null);
    }
}
