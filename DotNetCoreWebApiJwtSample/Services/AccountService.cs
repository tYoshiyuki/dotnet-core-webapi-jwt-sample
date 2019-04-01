using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(UserRequestModel user);
        Task<LoginResponseModel> Login(LoginRequestModel model);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;

        public AccountService(UserManager<IdentityUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<IdentityResult> Create(UserRequestModel user)
        {
            return await _userManager.CreateAsync(new IdentityUser { UserName = user.UserName, Email = user.Email }, user.Password);
        }

        /// <summary>
        /// Validate user id, password then generate token (as mean of login)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LoginResponseModel> Login(LoginRequestModel model)
        {
            var response = new LoginResponseModel();

            // Check user
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // Check password
                bool isPasswordOk = await _userManager.CheckPasswordAsync(user, model.Password);

                if (isPasswordOk)
                {
                    // Get roles
                    var roles = await _userManager.GetRolesAsync(user);

                    // If sucess then generate token
                    response.Token = _jwtService.GenerateEncodedToken(user.UserName, roles);
                }
            }
            return response;
        }
    }
}
