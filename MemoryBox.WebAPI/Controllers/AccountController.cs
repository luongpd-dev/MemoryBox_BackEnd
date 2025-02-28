using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Accounts;
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
        public async Task<IActionResult> Login(LoginRequest request)
        {

            var results = await _accountService.Login(request);
            return CustomResult("Đăng nhập thành công.", results);
        }
    }
}
