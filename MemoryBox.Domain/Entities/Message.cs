using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public Account User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime OpenDate { get; set; }
        public string? PasswordHash { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTimeOffset CreatedAt { get; set; } = CoreHelper.SystemTimeNow;
        public DateTimeOffset UpdatedAt { get; set; } = CoreHelper.SystemTimeNow;
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

    }
}
