using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using DotNetCoreWebApiJwtSample.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<IdentityUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("/Account/CreateUser")]
        public async Task<ActionResult> Create([FromBody] UserRequestModel user)
        {
            var result = await _accountService.Create(user);
            return result.Succeeded ? Ok() : GetErrorResult(result);
        }

        [HttpPost]
        [Route("/Account/AddToRole")]
        public async Task<ActionResult> AddToRole([FromBody] AddToRoleRequestModel model)
        {
            var result = await _accountService.AddToRole(model.Email, model.RoleName);
            return result.Succeeded ? Ok() : GetErrorResult(result);
        }

        [HttpGet]
        [Route("/Account/GetUsers")]
        public List<UserResponseModel> GetUsers()
        {
            return _accountService.GetList().Select(_ => new UserResponseModel
            {
                Email = _.Email,
                UserName = _.UserName
            }).ToList();
        }

        [HttpPost]
        [Route("/Account/Login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel login)
        {
            var response = await _accountService.Login(login);

            if (string.IsNullOrEmpty(response.Token))
            {
                return BadRequest("Invalid email or password");
            }
            return response;
        }

        [HttpPost]
        [Route("/Account/SignInWithGoogle")]
        public IActionResult SignInWithGoogle()
        {
            var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(HandleExternalLogin)));
            return Challenge(authenticationProperties, "Google");
        }

        [HttpGet]
        [Route("/Account/HandleExternalLogin")]
        public async Task<ActionResult<LoginResponseModel>> HandleExternalLogin()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return BadRequest();

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (!result.Succeeded) // user does not exist yet
            {
                await _accountService.Create(email);
            }

            var response = await _accountService.ExternalLogin(email, info);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return response;
        }
    }
}
