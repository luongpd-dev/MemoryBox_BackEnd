using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.Accounts
{
    public class AccountResponse : IMapFrom<Account>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        //public List<string> Roles { get; set; }
    }
}
