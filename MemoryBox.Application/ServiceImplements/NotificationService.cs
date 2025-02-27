using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Notifications;
using MemoryBox.Application.ViewModels.Response.Notifications;
using MemoryBox.Domain.CustomException;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryBox.Application.ServiceImplements
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NotificationResponse> CreateNotification(NotificationRequest request)
        {
            var user = await _unitOfWork.AccountRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new CustomException.DataNotFoundException("User not found");
            }

            var notification = _mapper.Map<Notification>(request);
            _unitOfWork.NotificationRepository.Insert(notification);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<NotificationResponse>(notification);
        }


        public async Task<bool> DeleteNotification(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
            if (notification == null)
                throw new CustomException.DataNotFoundException("Notification not found");

            _unitOfWork.NotificationRepository.Delete(notification);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<NotificationResponse> GetNotificationById(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
            if (notification == null)
                throw new CustomException.DataNotFoundException("Notification not found");

            return _mapper.Map<NotificationResponse>(notification);
        }

        public async Task<IEnumerable<NotificationResponse>> GetNotifications()
        {
            var notifications = _unitOfWork.NotificationRepository.GetAll();
            return _mapper.Map<IEnumerable<NotificationResponse>>(notifications);
        }

        public async Task<NotificationResponse> UpdateNotification(Guid id, UpdateNotificationRequest request)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
            if (notification == null)
                throw new CustomException.DataNotFoundException("Notification not found");

            _mapper.Map(request, notification);
            _unitOfWork.NotificationRepository.Update(notification);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<NotificationResponse>(notification);
        }
    }
}