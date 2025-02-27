using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Recipients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/recipients")]
    [ApiController]
    public class RecipientController : BaseController
    {
        private readonly IRecipientService _recipientService;

        public RecipientController(IRecipientService recipientService)
        {
            _recipientService = recipientService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var recipients = await _recipientService.GetRecipients();
            return CustomResult("Loading OK!", recipients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipientInfo(Guid id)
        {
            var recipient = await _recipientService.GetRecipientById(id);
            return CustomResult("Loading OK!", recipient);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RecipientRequest request)
        {
            var createdRecipient = await _recipientService.CreateRecipient(request);
            return CustomResult("Recipient created successfully", createdRecipient);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecipientRequest request)
        {
            var updatedRecipient = await _recipientService.UpdateRecipient(id, request);
            return CustomResult("Recipient updated successfully", updatedRecipient);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _recipientService.DeleteRecipient(id);
            return CustomResult("Recipient deleted successfully", isDeleted);
        }
    }
}
