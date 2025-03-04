using MemoryBox.Application.ViewModels.Request.AccountSubscriptions;
using MemoryBox.Application.ViewModels.Response.AccountSubscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IAccountSubscriptionService
    {
        Task<AccountSubscriptionResponse> CreateSubscription(AccountSubscriptionRequest request);
        Task<AccountSubscriptionResponse> GetSubscriptionById(Guid id);
        Task<IEnumerable<AccountSubscriptionResponse>> GetSubscriptionsByAccountId(Guid accountId);
        Task<bool> CancelSubscription(Guid id);
    }
}
