namespace PS.Common.Domain.Settings.Abstract;
public interface ICosmosDbSettings
{
    string CosmosDatabaseName { get; init; }
    string CosmosContainerName { get; init; }
    string CosmosConnectionString { get; init; }
}
