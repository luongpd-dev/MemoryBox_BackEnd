using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.AccountSubscriptions
{
    public class AccountSubscriptionResponse : IMapFrom<AccountSubscription>
    {
        public Guid AccountSubscriptionId { get; set; }

        public Guid AccountId { get; set; }

        public Guid SubscriptionPlanId { get; set; }

        public DateTimeOffset StartDate { get; set; } 
        public DateTimeOffset ExpiryDate { get; set; }

        public bool IsActive { get; set; }
    }
}
