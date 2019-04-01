using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using DotNetCoreWebApiJwtSample.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DotNetCoreWebApiJwtSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
