using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreWebApiJwtSample.RequestModels;
using DotNetCoreWebApiJwtSample.ResponseModels;
using DotNetCoreWebApiJwtSample.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreWebApiJwtSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Route("/Role/CreateRole")]
        public async Task<ActionResult> Create([FromBody] RoleRequestModel model)
        {
            var result = await _roleService.Create(model.RoleName);
            return result.Succeeded ? Ok() : GetErrorResult(result);
        }

        [HttpGet]
        [Route("/Role/GetRoles")]
        public List<RoleResponseModel> GetList()
        {
            return _roleService.GetList().Select(_ => new RoleResponseModel { RoleName = _.Name }).ToList();
        }
    }
}