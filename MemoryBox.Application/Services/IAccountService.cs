using MemoryBox.Application.ViewModels.Request.Accounts;
using MemoryBox.Application.ViewModels.Response.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IAccountService
    {
        //Get all
        Task<AccountResponse> GetAccountInfoAsync(Guid accountId);
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();

        Task<bool> CreateAccountAsync(AccountRequest request);
        Task<LoginResponse> Login(LoginRequest login);
    }
}
