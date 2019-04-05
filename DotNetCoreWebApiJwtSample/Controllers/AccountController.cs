using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using DotNetCoreWebApiJwtSample.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
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
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
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
            if (!result.Succeeded) //user does not exist yet
            {                
                await _accountService.Create(email);
            }

            var response = await _accountService.ExternalLogin(email, info);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return response;
        }

        // TODO 基底クラスに移動
        protected ActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null || result.Succeeded) throw new ArgumentException();


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            return BadRequest(ModelState);
        }
    }
}
