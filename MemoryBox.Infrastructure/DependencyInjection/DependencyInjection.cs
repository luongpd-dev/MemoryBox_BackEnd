using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using MemoryBox.Infrastructure.Data;
using MemoryBox.Infrastructure.Integrations.SignalR;
using MemoryBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryBox.Infrastructure.Authentication;
using MemoryBox.Application.Mapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ServiceImplements;
using MemoryBox.Infrastructure.Integrations.Firebase;

namespace MemoryBox.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {

        public static IServiceCollection InfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);

            services.AddRepositories();

            //services.AddRabbitMQServices(configuration);

            //services.AddQuartzAndSchedule();

            services.AddServices();


            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true; // Hiển thị lỗi chi tiết
            });
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddJWT(configuration);

            services.AddUtils();

            services.AddExternalServices();

            services.AddPayOS(configuration);

            services.AddAutoMapper(typeof(MappingProfile));


            return services;
        }

        //Database
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)),
                ServiceLifetime.Scoped);
        }

        //Repository
        public static void AddRepositories(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IRecipientService, RecipientService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAccountService, AccountService>();

        }

        //AddAuthentication
        public static void AddJWT(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<Account, Role>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"]; // Lấy token từ query string

                        // Nếu request là cho SignalR thì sử dụng token từ query
                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/notificationHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };


            });

            services.Configure<IdentityOptions>(options =>
            {
                // Set your desired password requirements here
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddScoped<IAuthentication, Authen>();
        }

        //Utils
        public static void AddUtils(this IServiceCollection services)
        {
        }

        //External
        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IFirebaseConfig, FirebaseConfig>();
        }

        //PayOS
        public static void AddPayOS(this IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
