using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Entities
{
    public class SubscriptionPlan
    {
        [Key]
        public Guid SubscriptionPlanId { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } 

        public string Description { get; set; } 

        [Required]
        public decimal Price { get; set; } 

        public int MaxUploadSizeMB { get; set; } 

        public bool IsUnlimited { get; set; } 

        public DateTimeOffset CreatedAt { get; set; } = CoreHelper.SystemTimeNow;
        public DateTimeOffset UpdateAt { get; set; } = CoreHelper.SystemTimeNow;
    }

}
