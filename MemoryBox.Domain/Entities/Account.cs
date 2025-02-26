using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class Account : IdentityUser<Guid>
    {
        public string? Avatar { get; set; }
        public string? EmailConfirmationOtp { get; set; }
        public DateTime? OtpExpiryTime { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
