using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Request.Notifications
{
    public class UpdateNotificationRequest : IMapFrom<Notification>
    {
        //public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }
    }
}
