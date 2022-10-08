using Microsoft.Extensions.Configuration;

namespace BrawlBuff.Infrastructure.Extensions
{
    public static class AzureKeyVaultExtension
    {
        public static void AddAzureKeyVault(this IConfigurationBuilder configurationBuilder)
        {
            var configuration = configurationBuilder.Build();
            var vaultUri = configuration["KeyVaults:brawlbuff-kv:Endpoint"];
            var clientId = configuration["BrawlBuff:ClientId"];
            var clientSecret = configuration["BrawlBuff:ClientSecret"];

            if (string.IsNullOrWhiteSpace(vaultUri) || string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentException("No Key Vault credentials provided.");
            }

            configurationBuilder.AddAzureKeyVault(vaultUri, clientId, clientSecret);
        }
    }
}
