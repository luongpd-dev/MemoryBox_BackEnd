using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.AccountSubscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/account-subscriptions")]
    [ApiController]
    public class AccountSubscriptionController : BaseController
    {
        private readonly IAccountSubscriptionService _accountSubscriptionService;

        public AccountSubscriptionController(IAccountSubscriptionService accountSubscriptionService)
        {
            _accountSubscriptionService = accountSubscriptionService;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetSubscriptions(Guid accountId)
        {
            var subscriptions = await _accountSubscriptionService.GetSubscriptionsByAccountId(accountId);
            return CustomResult("Lấy danh sách đăng ký thành công", subscriptions);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] AccountSubscriptionRequest request)
        {
            var subscription = await _accountSubscriptionService.CreateSubscription(request);
            return CustomResult("Đăng ký gói thành công", subscription);
        }
    }
}
