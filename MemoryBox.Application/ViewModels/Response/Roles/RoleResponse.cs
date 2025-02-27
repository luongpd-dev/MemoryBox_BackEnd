using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.Roles
{
    public class RoleResponse : IMapFrom<Role>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
