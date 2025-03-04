using MemoryBox.Application.Mapper;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ViewModels.Response.PaymentTransactions
{
    public class PaymentTransactionResponse:IMapFrom<PaymentTransaction>
    {
        public Guid PaymentTransactionId { get; set; }

        public Guid AccountId { get; set; }

        public Guid SubscriptionPlanId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
