using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.Messages
{
    public class MessageResponse : IMapFrom<Message>
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public Account User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime OpenDate { get; set; }
        public string? PasswordHash { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset UpdatedAt { get; set; } 
    }
}
