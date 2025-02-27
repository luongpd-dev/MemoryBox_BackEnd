using MemoryBox.Application.ViewModels.Request.Messages;
using MemoryBox.Application.ViewModels.Response.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public interface IMessageService
    {
        Task<MessageResponse> GetMessageById(Guid id);
        Task<IEnumerable<MessageResponse>> GetMessages();
        Task<MessageResponse> CreateMessage (MessageRequest messageRequest);
        Task<MessageResponse> UpdateMessage(Guid id, UpdateMessageRequest messageRequest);
        Task<bool> DeleteMessage (Guid id);
    }
}
