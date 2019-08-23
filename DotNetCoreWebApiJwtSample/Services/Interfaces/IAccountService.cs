using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreWebApiJwtSample.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(UserRequestModel user);
        Task<IdentityResult> Create(string email);
        Task<LoginResponseModel> Login(LoginRequestModel model);
        Task<LoginResponseModel> ExternalLogin(string email, ExternalLoginInfo info);

    }
}
