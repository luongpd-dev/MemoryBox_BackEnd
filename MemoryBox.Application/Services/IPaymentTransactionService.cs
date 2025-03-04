using MemoryBox.Application.ViewModels.Request.PaymentTransactions;
using MemoryBox.Application.ViewModels.Response.PaymentTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransactionResponse> CreateTransaction(PaymentTransactionRequest request);
        Task<bool> DeleteTransaction(Guid id);
        Task<PaymentTransactionResponse> GetTransactionById(Guid id);
        Task<IEnumerable<PaymentTransactionResponse>> GetAllTransactions();
        Task<PaymentTransactionResponse> UpdateTransactionStatus(Guid id, string status);

    }
}
