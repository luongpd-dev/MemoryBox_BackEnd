using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.SubscriptionPlans;
using MemoryBox.Application.ViewModels.Response.SubscriptionPlans;
using MemoryBox.Domain.CustomException;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ServiceImplements
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionPlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubscriptionPlanResponse> CreateSubscriptionPlan(SubscriptionPlanRequest request)
        {
            var plan = _mapper.Map<SubscriptionPlan>(request);
            _unitOfWork.SubscriptionPlanRepository.Insert(plan);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<SubscriptionPlanResponse>(plan);
        }

        public async Task<SubscriptionPlanResponse> GetSubscriptionPlanById(Guid id)
        {
            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null)
                throw new CustomException.DataNotFoundException("Subscription plan not found");

            return _mapper.Map<SubscriptionPlanResponse>(plan);
        }

        public async Task<IEnumerable<SubscriptionPlanResponse>> GetAllSubscriptionPlans()
        {
            var plans = await _unitOfWork.SubscriptionPlanRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubscriptionPlanResponse>>(plans);
        }

        public async Task<SubscriptionPlanResponse> UpdateSubscriptionPlan(Guid id, SubscriptionPlanRequest request)
        {
            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null)
                throw new CustomException.DataNotFoundException("Subscription plan not found");

            _mapper.Map(request, plan);
            _unitOfWork.SubscriptionPlanRepository.Update(plan);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<SubscriptionPlanResponse>(plan);
        }

        public async Task<bool> DeleteSubscriptionPlan(Guid id)
        {
            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null)
                throw new CustomException.DataNotFoundException("Subscription plan not found");

            _unitOfWork.SubscriptionPlanRepository.Delete(plan);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
