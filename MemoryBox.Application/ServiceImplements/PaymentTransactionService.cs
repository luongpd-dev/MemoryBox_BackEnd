using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.PaymentTransactions;
using MemoryBox.Application.ViewModels.Response.PaymentTransactions;
using MemoryBox.Domain.CustomException;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MemoryBox.Domain.CustomException.CustomException;

namespace MemoryBox.Application.ServiceImplements
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaymentTransactionResponse> CreateTransaction(PaymentTransactionRequest request)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(request.AccountId);
            if (account == null)
            {
                throw new DataNotFoundException("User not found");
            }

            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanId);
            if (plan == null)
            {
                throw new DataNotFoundException("Subscription plan not found");
            }

            var transaction = _mapper.Map<PaymentTransaction>(request);
            transaction.Status = "Pending";
            _unitOfWork.PaymentTransactionRepository.Insert(transaction);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PaymentTransactionResponse>(transaction);
        }

        public async Task<bool> DeleteTransaction(Guid id)
        {
            var transaction = await _unitOfWork.PaymentTransactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new DataNotFoundException("Transaction not found");
            }

            _unitOfWork.PaymentTransactionRepository.Delete(transaction);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<PaymentTransactionResponse> GetTransactionById(Guid id)
        {
            var transaction = await _unitOfWork.PaymentTransactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new DataNotFoundException("Transaction not found");
            }
            return _mapper.Map<PaymentTransactionResponse>(transaction);
        }

        public async Task<IEnumerable<PaymentTransactionResponse>> GetAllTransactions()
        {
            var transactions = await _unitOfWork.PaymentTransactionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentTransactionResponse>>(transactions);
        }

        public async Task<PaymentTransactionResponse> UpdateTransactionStatus(Guid id, string status)
        {
            var transaction = await _unitOfWork.PaymentTransactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new DataNotFoundException("Transaction not found");
            }

            transaction.Status = status;
            _unitOfWork.PaymentTransactionRepository.Update(transaction);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PaymentTransactionResponse>(transaction);
        }
    }
}
