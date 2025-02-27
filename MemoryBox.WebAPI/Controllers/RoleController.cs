using CoreApiResponse;
using MemoryBox.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetRoles();
            return CustomResult("Roles loaded successfully", roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleById(id);
            return CustomResult("Role loaded successfully", role);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] string roleName)
        {
            var createdRole = await _roleService.CreateRole(roleName);
            return CustomResult("Role created successfully", createdRole);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _roleService.DeleteRole(id);
            return CustomResult("Role deleted successfully", isDeleted);
        }
    }
}