using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DotNetCoreWebApiJwtSample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        [SuppressMessage("Performance", "CA1822:メンバーを static に設定します")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}
