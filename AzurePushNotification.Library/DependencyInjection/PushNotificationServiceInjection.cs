using AzurePushNotification.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AzurePushNotification.DependencyInjection
{
    public static class PushNotificationServiceInjection
    {
        public static IServiceCollection AddPushNotificationService(this IServiceCollection services)
        {
            services.AddSingleton<IAzurePushNotificationService, AzurePushNotificationService>();
            return services;
        }
    }
}
