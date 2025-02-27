using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Request.Messages
{
    public class MessageRequest : IMapFrom<Message>
    {
        public Guid AccountId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime OpenDate { get; set; }
        public string? PasswordHash { get; set; }
    }
}
