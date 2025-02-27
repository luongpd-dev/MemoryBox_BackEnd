using MemoryBox.Application.ViewModels.Response.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetRoles();
        Task<RoleResponse> GetRoleById(Guid id);
        Task<RoleResponse> CreateRole(string roleName);
        Task<bool> DeleteRole(Guid id);

    }
}
