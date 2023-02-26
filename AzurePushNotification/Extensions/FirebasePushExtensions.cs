using PN.Options;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PN.Extensions
{
    public static class FirebasePushExtensions
    {
        public static IServiceCollection AddFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            var firebaseCredential = configuration
                .GetSection(nameof(GoogleCredentials))
                .Get<GoogleCredentials>();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJsonParameters(new JsonCredentialParameters
                {
                    TokenUrl = firebaseCredential.TokenUri,
                    Type = firebaseCredential.Type,
                    ClientId = firebaseCredential.ClientId,
                    ClientEmail = firebaseCredential.ClientEmail,
                    ProjectId = firebaseCredential.ProjectId,
                    PrivateKey = firebaseCredential.PrivateKey,
                    PrivateKeyId = firebaseCredential.PrivateKeyId,
                }),
            });

            return services;
        }
    }
}
