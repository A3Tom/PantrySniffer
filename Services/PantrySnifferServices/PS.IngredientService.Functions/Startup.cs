using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PS.Common.Domain.Extensions;
using PS.Common.Domain.Settings;
using PS.Common.Queries.Ingredient;
using PS.IngredientService.Application.Handlers.Queries;
using PS.IngredientService.Data.Repositories;
using PS.IngredientService.Functions;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace PS.IngredientService.Functions;
internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        ConfigureSettings(builder.Services);
        ConfigureLogging(builder.Services);
        ConfigureMediator(builder.Services);

        ConfigureServiceLifetimeScopes(builder.Services);
    }

    private static void ConfigureSettings(IServiceCollection services)
    {
        var appConfig = services.BuildConfigurationRoot();
        var settings = appConfig.Get<PantrySettings>();

        if (settings == null)
            throw new NullReferenceException("Cannot instantiate settings, please check your Azure application config or local.settings.json file");

        settings.AddToServiceConfiguration(services);
    }

    private static void ConfigureMediator(IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<SearchIngredientsQuery>());
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<SearchIngredientsHandler>());
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<UpsertIngredientCommand>());
    }

    private static void ConfigureLogging(IServiceCollection services)
    {
        services.AddLogging();
    }

    private static void ConfigureServiceLifetimeScopes(IServiceCollection services)
    {
        services.AddTransient<IIngredientsRepository, IngredientsRepository>();
    }
}
