using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.RequestModels
{
    public class AddToRoleRequestModel
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
