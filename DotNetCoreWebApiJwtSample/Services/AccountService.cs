using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(UserRequestModel user);
        Task<IdentityResult> Create(string email);
        Task<LoginResponseModel> Login(LoginRequestModel model);
        Task<LoginResponseModel> ExternalLogin(string email, ExternalLoginInfo info);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(UserManager<IdentityUser> userManager, IJwtService jwtService, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Create(UserRequestModel user)
        {
            return await _userManager.CreateAsync(new IdentityUser { UserName = user.UserName, Email = user.Email }, user.Password);
        }

        public async Task<IdentityResult> Create(string email)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            return await _userManager.CreateAsync(new IdentityUser { UserName = email, Email = email }, "password");
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

 
        public async Task<LoginResponseModel> ExternalLogin(string email, ExternalLoginInfo info)
        {
            var response = new LoginResponseModel();

            // Check user
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userManager.AddLoginAsync(user, info);
                // Get roles
                var roles = await _userManager.GetRolesAsync(user);

                // If sucess then generate token
                response.Token = _jwtService.GenerateEncodedToken(user.UserName, roles);
            }
            return response;
        }
    }
}
