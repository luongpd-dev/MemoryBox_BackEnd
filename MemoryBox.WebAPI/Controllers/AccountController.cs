using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Accounts;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : BaseController
    {
        public readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;

        }

        [HttpGet("{accountId}")]

        public async Task<IActionResult> GetUserInfo(Guid accountId)
        {
            var account = await _accountService.GetAccountInfoAsync(accountId);
            return CustomResult("Tải dữ liệu thành công.", account);
        }

        [HttpGet("get-all")]

        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return CustomResult("Tải dữ liệu thành công.", accounts);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromForm] AccountRequest request)
        {
            var account = await _accountService.CreateAccountAsync(request);
            return CustomResult("Tạo 1 account thành công", account);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Application.ViewModels.Request.Accounts.LoginRequest request)
        {

            var results = await _accountService.Login(request);
            return CustomResult("Đăng nhập thành công.", results);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Application.ViewModels.Request.Accounts.RegisterRequest request)
        {
            var results = await _accountService.RegisterUser(request);
            return CustomResult("Tạo tài khoản thành công. Vui lòng kiểm tra email để xác nhận tài khoản trước khi đăng nhập.", results);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {

            await _accountService.ForgotPasswordAsync(request.Email);
            return CustomResult("Email khôi phục mật khẩu đã được gửi.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {

            await _accountService.ResetPasswordAsync(request);
            return CustomResult("Đặt lại mật khẩu thành công.");

        }

        [HttpPost("verify-OTP")]
        public async Task<IActionResult> VerifyOTP(string email, string otp)
        {
            var result = await _accountService.VerifyOtpAsync(email, otp);
            return CustomResult("Xác thực OTP thành công.", result);
        }

        [HttpPost("resend-OTP")]
        public async Task<IActionResult> ResendOTP(string email)
        {
            var result = await _accountService.ResendOtpAsync(email);
            return CustomResult("OTP đã được gửi lại.", result);
        }
    }
}
