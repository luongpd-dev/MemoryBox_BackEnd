using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.Recipients
{
    public class RecipientResponse : IMapFrom<Recipient>
    {
        public Guid RecipientId { get; set; }
        public Guid MessageId { get; set; }
        public Message Message { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset UpdatedAt { get; set; } 
    }
}
