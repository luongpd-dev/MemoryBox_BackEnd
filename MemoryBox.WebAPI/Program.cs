using MemoryBox.WebAPI.Middleware;
using MemoryBox.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using MemoryBox.WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins(
            "https://capstone-websystem.vercel.app",
            "http://127.0.0.1:5500",
            "http://localhost:3000",
            "http://localhost:5174",
            "http://10.0.2.2:5173",  // Genymotion Emulator  
            "http://10.0.2.2:8081",  // Android Emulator  
            "http://10.0.3.2:8081",  // Genymotion Emulator  
            "http://192.168.1.100:5173",  // LAN IP  
            "http://192.168.1.101:5173",  // LAN IP của thiết bị khác  
            "http://192.168.1.100:8081",  // LAN IP  
            "http://192.168.1.101:8081",  // LAN IP của thiết bị khác  
            "http://expo.dev",  // Thêm domain của Expo nếu cần  
            "http://192.168.1.100:19000"  // Expo local server  
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();

    });
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "AVR API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    option.SchemaFilter<OptionalArraySchemaFilter>();
});

builder.Services.InfrastructureService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MessageBox API V1");
        c.RoutePrefix = string.Empty;
        c.EnableTryItOutByDefault();
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
