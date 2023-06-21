
using Microsoft.Azure.Cosmos;
using Database = Microsoft.Azure.Cosmos.Database;

namespace DataUploader.Services;

internal static class CosmosService
{
    public static async Task<Database> RetrieveDatabaseAsync(string? endpoint, string? key, string databaseName)
    {
        CosmosSerializationOptions serializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        };

        CosmosClientOptions options = new()
        {
            AllowBulkExecution = true,
            SerializerOptions = serializerOptions,
            MaxRetryAttemptsOnRateLimitedRequests = 10
        };

        CosmosClient client = new(
            accountEndpoint: endpoint,
            authKeyOrResourceToken: key,
            clientOptions: options
        );

        return await client.CreateDatabaseIfNotExistsAsync(
            id: databaseName
        );
    }
}