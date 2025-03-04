using MemoryBox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Account> AccountRepository { get; }
        public IGenericRepository<Attachment> AttachmentRepository { get; }
        public IGenericRepository<Message> MessageRepository { get; }
        public IGenericRepository<Notification> NotificationRepository { get; }
        public IGenericRepository<Recipient> RecipientRepository { get; }
        public IGenericRepository<Role> RoleRepository { get; }
        public IGenericRepository<Statistic> StatisticRepository { get; }
        public IGenericRepository<PaymentTransaction> PaymentTransactionRepository { get; }
        public IGenericRepository<AccountSubscription> AccountSubscriptionRepository { get; }
        public IGenericRepository<SubscriptionPlan> SubscriptionPlanRepository { get; }

        void Save();
        Task SaveAsync();
        void Dispose();
        Task DisposeAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
