using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.Notifications
{
    public class NotificationResponse : IMapFrom<Notification>
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; } 
        public DateTime SendAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } 
}
}
