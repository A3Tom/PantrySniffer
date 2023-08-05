using Microsoft.Extensions.DependencyInjection;
using PS.Common.Domain.Settings.Abstract;

namespace PS.Common.Domain.Settings;
public class PantrySettings : ICosmosDbSettings
{
    public string CosmosDatabaseName { get; init; } = string.Empty;
    public string CosmosContainerName { get; init; } = string.Empty;
    public string CosmosConnectionString { get; init; } = string.Empty;

    public void AddToServiceConfiguration(IServiceCollection services)
    {
        services.AddSingleton<ICosmosDbSettings>(this);
    }
}
