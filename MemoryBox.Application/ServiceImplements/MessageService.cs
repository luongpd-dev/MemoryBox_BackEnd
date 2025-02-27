using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Messages;
using MemoryBox.Application.ViewModels.Response.Messages;
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
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;

        }

        public async Task<MessageResponse> CreateMessage(MessageRequest messageRequest)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(messageRequest.AccountId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("User not found");
            }
            var message = _mapper.Map<Message>(messageRequest);
            message.AccountId = messageRequest.AccountId;
            _unitOfWork.MessageRepository.Insert(message);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<MessageResponse>(message);

        }

        public async Task<bool> DeleteMessage(Guid id)
        {
           var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
           if (message == null)
           {
               throw new CustomException.DataNotFoundException("Message not found");
           }
            _unitOfWork.MessageRepository.Delete(message);
            await _unitOfWork.SaveAsync();
           return true;
        }

        //GetById
        public async Task<MessageResponse> GetMessageById(Guid id)
        {
            var message = _unitOfWork.MessageRepository.GetByID(id);
            if (message == null)
            {
                throw new CustomException.DataNotFoundException("This message not foud");
            }
            return _mapper.Map<MessageResponse>(message);
        }

        //GetAll
        public async Task<IEnumerable<MessageResponse>> GetMessages()
        {
            var messages = _unitOfWork.MessageRepository.GetAll();
            if (messages == null)
            {
                throw new CustomException.DataNotFoundException("Message empty");
            }
            return _mapper.Map<IEnumerable<MessageResponse>>(messages);
        }



        //Update
        public async Task<MessageResponse> UpdateMessage(UpdateMessageRequest messageRequest)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(messageRequest.MessageId);
            if (message == null)
            {
                throw new CustomException.DataNotFoundException("Message not found");
            }

            _mapper.Map(messageRequest, message);
            _unitOfWork.MessageRepository.Update(message);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<MessageResponse>(message);
        }
    }
}
