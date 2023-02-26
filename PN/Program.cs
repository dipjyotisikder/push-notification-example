using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PN.Constants;
using PN.Library.Models;
using PN.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddControllers();

        builder.Services
            .Configure<NotificationHubOptions>(
                builder.Configuration.GetSection(nameof(NotificationHubOptions)));

        builder.Services.AddSingleton<IAzurePushNotificationService, AzurePushNotificationService>();

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJsonParameters(builder.Configuration
                .GetSection(CommonConstants.GOOGLE_CREDENTIAL_CONFIGURATION_KEY)
                .Get<JsonCredentialParameters>()),
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}