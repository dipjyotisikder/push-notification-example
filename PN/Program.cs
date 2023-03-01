using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PN.Constants;
using PN.Firestore.Entities;
using PN.Firestore.Repositories;
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

        builder.Services
            .AddSingleton<IAzurePushNotificationService, AzurePushNotificationService>();

        builder.Services
            .AddScoped(typeof(IFirebaseRepository<>), typeof(FirebaseRepository<>))
            .AddScoped<IUserRepository, UserRepository>();

        var googleCredential = builder.Configuration
                        .GetSection(CommonConstants.GOOGLE_CREDENTIAL_CONFIGURATION_KEY)
                        .Get<JsonCredentialParameters>();
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJsonParameters(googleCredential),
        });

        builder.Services.AddScoped(x => new FirestoreDbBuilder
        {
            ProjectId = googleCredential.ProjectId,
            GoogleCredential = GoogleCredential.FromJsonParameters(googleCredential),
        }.Build());

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