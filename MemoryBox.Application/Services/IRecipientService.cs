using MemoryBox.Application.ViewModels.Request.Messages;
using MemoryBox.Application.ViewModels.Request.Recipients;
using MemoryBox.Application.ViewModels.Response.Messages;
using MemoryBox.Application.ViewModels.Response.Recipients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IRecipientService
    {
        Task<RecipientResponse> GetRecipientById(Guid id);
        Task<IEnumerable<RecipientResponse>> GetRecipients();
        Task<RecipientResponse> CreateRecipient(RecipientRequest request);
        Task<RecipientResponse> UpdateRecipient(Guid id, UpdateRecipientRequest request);
        Task<bool> DeleteRecipient(Guid id);
    }
}
