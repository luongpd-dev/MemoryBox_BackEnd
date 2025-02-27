using MemoryBox.Application.ViewModels.Request.Notifications;
using MemoryBox.Application.ViewModels.Response.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface INotificationService
    {
        Task<NotificationResponse> CreateNotification(NotificationRequest request);
        Task<bool> DeleteNotification(Guid id);
        Task<NotificationResponse> GetNotificationById(Guid id);
        Task<IEnumerable<NotificationResponse>> GetNotifications();
        Task<NotificationResponse> UpdateNotification(Guid id, UpdateNotificationRequest request);
    }
}
