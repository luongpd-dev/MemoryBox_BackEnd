using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Request.PaymentTransactions
{
    public class PaymentTransactionRequest : IMapFrom<PaymentTransaction>
    {
        public Guid AccountId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public decimal Amount { get; set; }
    }
}
