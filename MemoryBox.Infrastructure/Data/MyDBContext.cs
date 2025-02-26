using MemoryBox.Domain.Entities;
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
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);

            builder.Entity<MemoryBox.Domain.Entities.Attachment>()
                .HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId);

            builder.Entity<Recipient>()
                .HasOne(r => r.Message)
                .WithMany(m => m.Recipients)
                .HasForeignKey(r => r.MessageId);

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            builder.Entity<Statistic>()
               .HasKey(s => s.StatId);

            builder.Entity<Statistic>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Statistic>(s => s.UserId);
        }

    }
}
