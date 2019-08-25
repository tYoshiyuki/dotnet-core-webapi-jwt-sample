using DotNetCoreWebApiJwtSample.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Create(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public List<IdentityRole> GetList()
        {
            return _roleManager.Roles.ToList();
        }
    }
}
