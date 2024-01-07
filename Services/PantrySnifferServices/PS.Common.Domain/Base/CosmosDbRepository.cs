using Microsoft.Azure.Cosmos;
using PS.Common.Domain.Abstract;
using PS.Common.Domain.Settings.Abstract;

namespace PS.Common.Domain.Base;
public abstract class CosmosDbRepository
{
    private const string PARTITION_KEY_PATH = "/PartitionKey";

    private readonly string _databaseName;
    private readonly string _containerName;
    private readonly CosmosClient _cosmosClient;

    public CosmosDbRepository(ICosmosDbSettings cosmosSettings)
    {
        _databaseName = cosmosSettings.CosmosDatabaseName;
        _containerName = cosmosSettings.CosmosContainerName;
        _cosmosClient = new(cosmosSettings.CosmosConnectionString);
    }

    public async Task<T> InsertEntityAsync<T>(T entity) where T : IHasPartitionKey
        => await ExceptionHandler(async () =>
        {
            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);
            return await container.CreateItemAsync(entity, new(entity.PartitionKey));
        });

    public async Task<T> UpdateEntityAsync<T>(T entity) where T : IHasPartitionKey
        => await ExceptionHandler(async () =>
        {
            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);
            var partitionKey = new PartitionKey(entity.PartitionKey);

            return await container.UpsertItemAsync(entity);
        });

    public async Task<T> DeleteEntityAsync<T>(T entity) where T : IHasPartitionKey
        => await ExceptionHandler(async () =>
        {
            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);
            var partitionKey = new PartitionKey(entity.PartitionKey);

            return await container.DeleteItemAsync<T>(entity.PartitionKey, partitionKey);
        });

    public async Task<List<T>> GetEntitiesByPartitionKeyAsync<T>(string partitionKey)
        => await ExceptionHandler(async () =>
        {
            List<T> result = new();

            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);

            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.PartitionKey = @partitionKey")
                .WithParameter("@partitionKey", partitionKey);

            FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                result.AddRange(response.Resource);
            }

            return result;
        });

    public async Task<List<T>> SearchEntitiesByName<T>(string searchTerm) where T : INamedEntity
        => await ExceptionHandler(async () =>
        {
            List<T> result = new();

            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);

            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE UPPER(c.Name) LIKE @searchTerm")
                .WithParameter("@searchTerm", $"%{searchTerm.ToUpper()}%");

            FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                result.AddRange(response.Resource);
            }

            return result;
        });

    public async Task<List<T>> ListEntitiesByType<T>(int ingredientType) where T : INamedEntity
        => await ExceptionHandler(async () =>
        {
            List<T> result = new();

            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);

            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.Type = @ingredientType")
                .WithParameter("@ingredientType", ingredientType);

            FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                result.AddRange(response.Resource);
            }

            return result;
        });

    public async Task<List<T>> SearchEntitiesById<T>(Guid entityId)
        => await ExceptionHandler(async () =>
        {
            List<T> result = new();

            var container = await BuildContainerInstance(_cosmosClient, _databaseName, _containerName);

            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.id = @entityId")
                .WithParameter("@entityId", entityId.ToString());

            FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                result.AddRange(response.Resource);
            }

            return result;
        });

    private async static Task<Container> BuildContainerInstance(CosmosClient client, string databaseName, string containerName)
    {
        Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        return await database.CreateContainerIfNotExistsAsync(containerName, PARTITION_KEY_PATH);
    }

    private static async Task<T> ExceptionHandler<T>(Func<Task<ItemResponse<T>>> cosmosTask)
    {
        try
        {
            return await cosmosTask.Invoke();
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error inserting entity: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Generic error: {ex.Message}");
            throw;
        }
    }
    private static async Task<List<T>> ExceptionHandler<T>(Func<Task<List<T>>> cosmosTask)
    {
        try
        {
            return await cosmosTask.Invoke();
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error inserting entity: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Generic error: {ex.Message}");
            throw;
        }
    }
}
