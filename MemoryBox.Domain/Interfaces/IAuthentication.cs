using MemoryBox.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Interfaces
{
    public interface IAuthentication
    {
        string GenerateJWTToken(Account account);
        Guid GetUserIdFromHttpContext(HttpContext httpContext);

        bool VerifyPassword(string providedPassword, string hashedPassword, Account account);

        string HashPassword(Account account, string password);
    }
}
