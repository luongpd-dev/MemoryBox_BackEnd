using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Recipients;
using MemoryBox.Application.ViewModels.Response.Recipients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ServiceImplements
{
    public class RecipientService : IRecipientService
    {
        public Task<RecipientResponse> CreateRecipient(RecipientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRecipient(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RecipientResponse> GetRecipientById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecipientResponse>> GetRecipients()
        {
            throw new NotImplementedException();
        }

        public Task<RecipientRequest> UpdateRecipient(UpdateRecipientRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
