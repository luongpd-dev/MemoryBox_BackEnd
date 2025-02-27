using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Recipients;
using MemoryBox.Application.ViewModels.Response.Recipients;
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
    public class RecipientService : IRecipientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public RecipientService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<RecipientResponse> CreateRecipient(RecipientRequest request)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(request.MessageId);
            if (message == null)
            {
                throw new CustomException.DataNotFoundException("Message not found");
            }
            var recipient = _mapper.Map<Recipient>(request);
            _unitOfWork.RecipientRepository.Insert(recipient);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<RecipientResponse>(recipient);
        }

        public async Task<bool> DeleteRecipient(Guid id)
        {
            var recipient = await _unitOfWork.RecipientRepository.GetByIdAsync(id);
            if (recipient == null)
            {
                throw new CustomException.DataNotFoundException("Recipient not found");
            }
            _unitOfWork.RecipientRepository.Delete(recipient);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<RecipientResponse> GetRecipientById(Guid id)
        {
            var recipient = await _unitOfWork.RecipientRepository.GetByIdAsync(id);
            if (recipient == null)
            {
                throw new CustomException.DataNotFoundException("Recipient not found");
            }
            return _mapper.Map<RecipientResponse>(recipient);
        }

        public async Task<IEnumerable<RecipientResponse>> GetRecipients()
        {
            var recipients = _unitOfWork.RecipientRepository.GetAll();
            if (!recipients.Any())
            {
                throw new CustomException.DataNotFoundException("No recipients found");
            }
            return _mapper.Map<IEnumerable<RecipientResponse>>(recipients);
        }

        public async Task<RecipientResponse> UpdateRecipient(Guid id, UpdateRecipientRequest request)
        {
            var recipient = await _unitOfWork.RecipientRepository.GetByIdAsync(id);
            if (recipient == null)
            {
                throw new CustomException.DataNotFoundException("Recipient not found");
            }

            _mapper.Map(request, recipient);
            _unitOfWork.RecipientRepository.Update(recipient);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<RecipientResponse>(recipient);
        }

       
    }
}
