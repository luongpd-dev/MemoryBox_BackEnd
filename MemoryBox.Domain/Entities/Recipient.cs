using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class Recipient
    {
        public Guid RecipientId { get; set; }
        public Guid MessageId { get; set; }
        [JsonIgnore]
        public virtual Message Message { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = CoreHelper.SystemTimeNow;
        public DateTimeOffset UpdatedAt { get; set; } = CoreHelper.SystemTimeNow;
    }
}
