
using BLL.Hubs;
using BLL.Interface;
using BLL.IService;
using BLL.Service;
using BLL.Services;
using BLL.Services.BLL.Services;
using DAL.Data;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.IService;
using Service.Service;

namespace SpeakAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IValidationHandleService, ValidationHandleService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAIService, AIService>();
            builder.Services.AddScoped<IPremiumSubscriptionService, PremiumSubscriptionService>();
            builder.Services.AddScoped<IPaymentService,PaymentService>();
            builder.Services.AddScoped<ITransactionService,TransactionService>();
            builder.Services.AddScoped<IValidationHandleService,ValidationHandleService>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();  

            builder.Services.AddScoped<ChatHub>();
            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            builder.Services.AddDbContext<SpeakAIContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
          
                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapHub<ChatHub>("/chatHub");

            app.MapControllers();
            app.UseCors("AllowAll");
            app.Run();
        }
    }
}
