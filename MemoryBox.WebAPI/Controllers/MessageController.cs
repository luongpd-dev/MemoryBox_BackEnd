using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/messages")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll() 
        {
            var messages = await _messageService.GetMessages();
            return CustomResult("Loading OK !", messages);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetMessageInfo(Guid id)
        {
            var message = await _messageService.GetMessageById(id);
            return CustomResult("Loading Ok!", message);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MessageRequest request)
        {
            var createdMessage = await _messageService.CreateMessage(request);
            return CustomResult("Message created successfully", createdMessage);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageRequest request)
        {
            var updatedMessage = await _messageService.UpdateMessage(request);
            return CustomResult("Message updated successfully", updatedMessage);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _messageService.DeleteMessage(id);
            return CustomResult("Message deleted successfully", isDeleted);
        }
    }
}
