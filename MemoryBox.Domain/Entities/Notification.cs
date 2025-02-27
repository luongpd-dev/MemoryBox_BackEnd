using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual Account User { get; set; }

        public string Content { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime SendAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = CoreHelper.SystemTimeNow;
    }
}
