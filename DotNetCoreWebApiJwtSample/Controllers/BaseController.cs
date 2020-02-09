using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreWebApiJwtSample.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (result.Succeeded) throw new ArgumentException(nameof(result));


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