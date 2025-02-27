using CoreApiResponse;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MemoryBox.WebAPI.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetNotifications();
            return CustomResult("Loading OK!", notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationInfo(Guid id)
        {
            var notification = await _notificationService.GetNotificationById(id);
            return CustomResult("Loading OK!", notification);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NotificationRequest request)
        {
            var createdNotification = await _notificationService.CreateNotification(request);
            return CustomResult("Notification created successfully", createdNotification);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNotificationRequest request)
        {
            var updatedNotification = await _notificationService.UpdateNotification(id, request);
            return CustomResult("Notification updated successfully", updatedNotification);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _notificationService.DeleteNotification(id);
            return CustomResult("Notification deleted successfully", isDeleted);
        }
    }
}
