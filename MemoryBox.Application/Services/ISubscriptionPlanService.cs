using MemoryBox.Application.ViewModels.Request.SubscriptionPlans;
using MemoryBox.Application.ViewModels.Response.SubscriptionPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface ISubscriptionPlanService
    {
        Task<SubscriptionPlanResponse> CreateSubscriptionPlan(SubscriptionPlanRequest request);
        Task<SubscriptionPlanResponse> GetSubscriptionPlanById(Guid id);
        Task<IEnumerable<SubscriptionPlanResponse>> GetAllSubscriptionPlans();
        Task<SubscriptionPlanResponse> UpdateSubscriptionPlan(Guid id, SubscriptionPlanRequest request);
        Task<bool> DeleteSubscriptionPlan(Guid id);
    }
}
