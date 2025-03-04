using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Request.AccountSubscriptions
{
    public class AccountSubscriptionRequest : IMapFrom<AccountSubscription>
    {
        public Guid AccountId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
    }
}
