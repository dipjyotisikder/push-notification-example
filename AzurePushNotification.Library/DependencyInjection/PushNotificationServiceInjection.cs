using PN.Services;
using Microsoft.Extensions.DependencyInjection;

namespace PN.DependencyInjection
{
    public static class PushNotificationServiceInjection
    {
        public static IServiceCollection AddAzurePushNotification(this IServiceCollection services)
        {
            services.AddSingleton<IAzurePushNotificationService, AzurePushNotificationService>();
            return services;
        }
    }
}
