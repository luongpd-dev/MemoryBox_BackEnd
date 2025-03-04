using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Request.SubscriptionPlans
{
    public class SubscriptionPlanRequest : IMapFrom<SubscriptionPlan>
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }

        public int MaxUploadSizeMB { get; set; }

        public bool IsUnlimited { get; set; }
    }
}
