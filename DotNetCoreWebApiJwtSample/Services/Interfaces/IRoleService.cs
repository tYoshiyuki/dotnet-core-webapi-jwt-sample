using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreWebApiJwtSample.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> Create(string roleName);

        List<IdentityRole> GetList();
    }
}
