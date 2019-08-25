using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(UserRequestModel user);
        Task<IdentityResult> Create(string email);
        Task<LoginResponseModel> Login(LoginRequestModel model);
        Task<LoginResponseModel> ExternalLogin(string email, ExternalLoginInfo info);
        List<IdentityUser> GetList();
        Task<IdentityResult> AddToRole(string email, string roleName);
    }
}
