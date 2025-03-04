using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.SubscriptionPlans;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/subscription-plans")]
    [ApiController]
    public class SubscriptionPlanController : BaseController
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(Guid id)
        {
            var plan = await _subscriptionPlanService.GetSubscriptionPlanById(id);
            return CustomResult("Lấy gói đăng ký thành công", plan);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _subscriptionPlanService.GetAllSubscriptionPlans();
            return CustomResult("Lấy danh sách gói đăng ký thành công", plans);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePlan([FromBody] SubscriptionPlanRequest request)
        {
            var plan = await _subscriptionPlanService.CreateSubscriptionPlan(request);
            return CustomResult("Tạo gói đăng ký thành công", plan);
        }
    }
}
