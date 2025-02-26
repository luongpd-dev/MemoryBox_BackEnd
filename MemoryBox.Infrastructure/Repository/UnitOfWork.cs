using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using MemoryBox.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork() { }

        private MyDbContext _context = new MyDbContext();
        private IGenericRepository<Account> _accountRepository;
        private IGenericRepository<Attachment> _attachmentRepository;
        private IGenericRepository<Message> _messageRepository;
        private IGenericRepository<Notification> _notificationRepository;
        private IGenericRepository<Recipient> _recipientRepository;
        private IGenericRepository<Role> _roleRepository;
        private IGenericRepository<Statistic> _statisticRepository;

        public IGenericRepository<Account> AccountRepository
        {
            get
            {

                if (_accountRepository == null)
                {
                    _accountRepository = new GenericRepository<Account>(_context);
                }
                return _accountRepository;
            }
        }

        public IGenericRepository<Attachment> AttachmentRepository
        {
            get
            {

                if (_attachmentRepository == null)
                {
                    _attachmentRepository = new GenericRepository<Attachment>(_context);
                }
                return _attachmentRepository;
            }
        }

        public IGenericRepository<Message> MessageRepository
        {
            get
            {

                if (_messageRepository == null)
                {
                    _messageRepository = new GenericRepository<Message>(_context);
                }
                return _messageRepository;
            }
        }
        public IGenericRepository<Notification> NotificationRepository
        {
            get
            {

                if (_notificationRepository == null)
                {
                    _notificationRepository = new GenericRepository<Notification>(_context);
                }
                return _notificationRepository;
            }
        }
        public IGenericRepository<Recipient> RecipientRepository
        {
            get
            {

                if (_recipientRepository == null)
                {
                    _recipientRepository = new GenericRepository<Recipient>(_context);
                }
                return _recipientRepository;
            }
        }
        public IGenericRepository<Role> RoleRepository
        {
            get
            {

                if (_roleRepository == null)
                {
                    _roleRepository = new GenericRepository<Role>(_context);
                }
                return _roleRepository;
            }
        }
        public IGenericRepository<Statistic> StatisticRepository
        {
            get
            {

                if (_statisticRepository == null)
                {
                    _statisticRepository = new GenericRepository<Statistic>(_context);
                }
                return _statisticRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        await _context.DisposeAsync();
                    }
                }
            }
            disposed = true;
        }

        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
    }

}
