using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.AccountSubscriptions;
using MemoryBox.Application.ViewModels.Response.AccountSubscriptions;
using MemoryBox.Domain.CustomException;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ServiceImplements
{
    public class AccountSubscriptionService : IAccountSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AccountSubscriptionService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }

        // Create subscription
        public async Task<AccountSubscriptionResponse> CreateSubscription(AccountSubscriptionRequest request)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new CustomException.DataNotFoundException("User not found");

            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId);
            if (plan == null)
                throw new CustomException.DataNotFoundException("Subscription plan not found");

            var subscription = _mapper.Map<AccountSubscription>(request);
            subscription.StartDate = DateTimeOffset.UtcNow;
            subscription.ExpiryDate = subscription.StartDate.AddMonths(1); // Ví dụ: gói có hạn trong 1 tháng
            subscription.IsActive = true;

            _unitOfWork.AccountSubscriptionRepository.Insert(subscription);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<AccountSubscriptionResponse>(subscription);
        }

        // Get subscription by ID
        public async Task<AccountSubscriptionResponse> GetSubscriptionById(Guid id)
        {
            var subscription = await _unitOfWork.AccountSubscriptionRepository.GetByIdAsync(id);
            if (subscription == null)
                throw new CustomException.DataNotFoundException("Subscription not found");

            return _mapper.Map<AccountSubscriptionResponse>(subscription);
        }

        // Get all subscriptions for a user
        public async Task<IEnumerable<AccountSubscriptionResponse>> GetSubscriptionsByAccountId(Guid accountId)
        {
            var subscriptions = await _unitOfWork.AccountSubscriptionRepository.GetByIdAsync(accountId);
            return _mapper.Map<IEnumerable<AccountSubscriptionResponse>>(subscriptions);
        }

        /*// Update subscription (Upgrade/Downgrade)
        public async Task<AccountSubscriptionResponse> UpdateSubscription(Guid id, UpdateSubscriptionRequest request)
        {
            var subscription = await _unitOfWork.AccountSubscriptionRepository.GetByIdAsync(id);
            if (subscription == null)
                throw new CustomException.DataNotFoundException("Subscription not found");

            var newPlan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.NewSubscriptionPlanId);
            if (newPlan == null)
                throw new CustomException.DataNotFoundException("New subscription plan not found");

            subscription.SubscriptionPlanId = request.NewSubscriptionPlanId;
            subscription.ExpiryDate = DateTimeOffset.UtcNow.AddMonths(1); // Reset thời gian hết hạn

            _unitOfWork.AccountSubscriptionRepository.Update(subscription);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<AccountSubscriptionResponse>(subscription);
        }*/

        // Cancel subscription
        public async Task<bool> CancelSubscription(Guid id)
        {
            var subscription = await _unitOfWork.AccountSubscriptionRepository.GetByIdAsync(id);
            if (subscription == null)
                throw new CustomException.DataNotFoundException("Subscription not found");

            subscription.IsActive = false;
            _unitOfWork.AccountSubscriptionRepository.Update(subscription);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
