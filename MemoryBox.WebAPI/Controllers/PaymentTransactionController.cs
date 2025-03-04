using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.PaymentTransactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/payment-transactions")]
    [ApiController]
    public class PaymentTransactionController : BaseController
    {
        private readonly IPaymentTransactionService _paymentTransactionService;

        public PaymentTransactionController(IPaymentTransactionService paymentTransactionService)
        {
            _paymentTransactionService = paymentTransactionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var transaction = await _paymentTransactionService.GetTransactionById(id);
            return CustomResult("Lấy thông tin giao dịch thành công", transaction);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _paymentTransactionService.GetAllTransactions();
            return CustomResult("Lấy danh sách giao dịch thành công", transactions);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] PaymentTransactionRequest request)
        {
            var transaction = await _paymentTransactionService.CreateTransaction(request);
            return CustomResult("Tạo giao dịch thành công", transaction);
        }
    }
}
