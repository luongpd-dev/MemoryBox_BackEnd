﻿using MemoryBox.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Infrastructure.Data
{
    public class MyDbContext : IdentityDbContext<Account, Role, Guid>
    {
        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MemoryBox.Domain.Entities.Attachment> Attachments { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Statistic> Statistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
            }

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Message>()
                .HasOne(m => m.Account)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MemoryBox.Domain.Entities.Attachment>()
                .HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Recipient>()
                .HasOne(r => r.Message)
                .WithMany(m => m.Recipients)
                .HasForeignKey(r => r.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Statistic>()
               .HasKey(s => s.StatId);

            builder.Entity<Statistic>()
                .HasOne(s => s.Account)
                .WithOne()
                .HasForeignKey<Statistic>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa UserSubscription và Account (1 user có thể có 1 gói)
            builder.Entity<AccountSubscription>()
                .HasOne(us => us.Account)
                .WithMany(a => a.AccountSubscriptions)
                .HasForeignKey(us => us.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa UserSubscription và SubscriptionPlan (Mỗi UserSubscription thuộc về 1 gói)
            builder.Entity<AccountSubscription>()
                .HasOne(us => us.Plan)
                .WithMany()
                .HasForeignKey(us => us.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa PaymentTransaction và Account (1 user có nhiều giao dịch)
            builder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Account)
                .WithMany(a => a.PaymentTransactions)
                .HasForeignKey(pt => pt.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa PaymentTransaction và SubscriptionPlan (Mỗi giao dịch thuộc về 1 gói)
            builder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Plan)
                .WithMany()
                .HasForeignKey(pt => pt.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
