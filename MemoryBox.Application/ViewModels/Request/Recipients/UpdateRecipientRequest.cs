using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Request.Recipients
{
    public class UpdateRecipientRequest : IMapFrom<Recipient>
    {
        public Guid MessageId { get; set; }
        /*public Message Message { get; set; }*/
        public string Email { get; set; }
    }
}
