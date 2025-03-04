using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.SubscriptionPlans
{
    public class SubscriptionPlanResponse : IMapFrom<SubscriptionPlan>
    {
        public Guid SubscriptionPlanId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }

        public int MaxUploadSizeMB { get; set; }

        public bool IsUnlimited { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
    }
}
