using Azure.Identity;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace PS.Common.Domain.Extensions;
public static class AppConfigExtensions
{
    private const string AZURE_ENVIRONMENT_FLAG_VARIABLE = "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING";
    private const string LOCAL_SETTINGS_FILE_NAME = "local.settings.json";
    private const string APP_CONFIG_LABEL = "PantrySniffer";

    public static IConfigurationRoot BuildConfigurationRoot(this IServiceCollection services)
    {
        var providers = new List<IConfigurationProvider>();

        var localConfig = BuildLocalAppConfig(services).Providers;
        providers.AddRange(localConfig);

        var azureAppConfigEndpoint = Environment.GetEnvironmentVariable("AppConfigEndpoint");
        if (!string.IsNullOrEmpty(azureAppConfigEndpoint))
        {
            var azureConfig = BuildAzureAppConfig(services, azureAppConfigEndpoint).Providers;
            providers.AddRange(azureConfig);
        }

        return new ConfigurationRoot(providers);
    }

    private static IConfigurationRoot BuildLocalAppConfig(IServiceCollection services)
    {
        var executionContextOptions = services.BuildServiceProvider()
            .GetService<IOptions<ExecutionContextOptions>>()
            ?.Value;

        var basePath = GetApplicationBasePath(executionContextOptions);

        return new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(LOCAL_SETTINGS_FILE_NAME, optional: true, reloadOnChange: false)
            .Build();
    }

    private static IConfigurationRoot BuildAzureAppConfig(IServiceCollection services, string appConfigEndpoint) =>
        new ConfigurationBuilder().AddAzureAppConfiguration(o =>
        {
            var opts = new DefaultAzureCredentialOptions()
            {
                ExcludeVisualStudioCredential = true,
                ExcludeInteractiveBrowserCredential = false
            };

            o.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential(opts))
                .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential(opts)))
                .Select("*", APP_CONFIG_LABEL);

            var configurationRefresher = o.GetRefresher();
            services.AddSingleton(configurationRefresher);
        }).Build();

    private static string GetApplicationBasePath(ExecutionContextOptions executionContext)
    {
        var basePath = executionContext?.AppDirectory;

        if (!string.IsNullOrWhiteSpace(basePath))
            return basePath;

        return !IsAzureEnvironment() ?
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!
            : $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";
    }
    private static bool IsAzureEnvironment() =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(AZURE_ENVIRONMENT_FLAG_VARIABLE));
}
