using MemoryBox.Application.ViewModels.Request.Accounts;
using MemoryBox.Application.ViewModels.Response.Accounts;
using MemoryBox.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IAccountService
    {
        //Get
        Task<AccountResponse> GetAccountInfoAsync(Guid accountId);
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();

        //Login
        Task<bool> RegisterUser(ViewModels.Request.Accounts.RegisterRequest registerRequest);
        Task<bool> CreateAccountAsync(AccountRequest request);
        Task<LoginResponse> Login(ViewModels.Request.Accounts.LoginRequest login);

        //Passwords
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);

        //OTP
        Task SendOtpAsync(Account account);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<bool> ResendOtpAsync(string email);
    }
}
