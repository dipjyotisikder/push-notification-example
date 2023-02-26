using Microsoft.Extensions.Configuration;

namespace PN.Options
{
    public class GoogleCredentials
    {
        [ConfigurationKeyName("type")]
        public string Type { get; set; }

        [ConfigurationKeyName("project_id")]
        public string ProjectId { get; set; }

        [ConfigurationKeyName("private_key_id")]
        public string PrivateKeyId { get; set; }

        [ConfigurationKeyName("private_key")]
        public string PrivateKey { get; set; }

        [ConfigurationKeyName("client_email")]
        public string ClientEmail { get; set; }

        [ConfigurationKeyName("client_id")]
        public string ClientId { get; set; }

        [ConfigurationKeyName("auth_uri")]
        public string AuthUri { get; set; }

        [ConfigurationKeyName("token_uri")]
        public string TokenUri { get; set; }

        [ConfigurationKeyName("auth_provider_x509_cert_url")]
        public string AuthProviderX509CertUrl { get; set; }

        [ConfigurationKeyName("client_x509_cert_url")]
        public string ClientX509CertUrl { get; set; }
    }
}
