using BLL.Hubs;
using BLL.Interface;
using BLL.Services;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Interface;
using Service.Services;

namespace BLL
{
    public static class BllDependencyInjection
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPremiumSubscriptionService, PremiumSubscriptionService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAIService, AIService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IValidationHandleService, ValidationHandleService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddHostedService<VoucherBackgroundService>();
            services.AddScoped<ChatHub>();
            services.AddDalServices(configuration);
            return services;
        }
    }
}
