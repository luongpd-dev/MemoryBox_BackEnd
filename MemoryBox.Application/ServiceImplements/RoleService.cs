using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Response.Roles;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryBox.Application.ServiceImplements
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponse>> GetRoles()
        {
            var roles = _roleManager.Roles;
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<RoleResponse> GetRoleById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> CreateRole(string roleName)
        {
            var role = new Role { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create role");
            }
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<bool> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
}
