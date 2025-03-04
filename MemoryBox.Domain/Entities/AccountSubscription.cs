using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryBox.Domain.Utils;

namespace MemoryBox.Domain.Entities
{
    public class AccountSubscription
    {
        [Key]
        public Guid AccountSubscriptionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AccountId { get; set; } 

        public virtual Account Account { get; set; }

        [Required]
        public Guid SubscriptionPlanId { get; set; } 

        public virtual SubscriptionPlan Plan { get; set; }

        public DateTimeOffset StartDate { get; set; } = CoreHelper.SystemTimeNow;
        public DateTimeOffset ExpiryDate { get; set; } 

        public bool IsActive { get; set; } = true; 
    }

}
