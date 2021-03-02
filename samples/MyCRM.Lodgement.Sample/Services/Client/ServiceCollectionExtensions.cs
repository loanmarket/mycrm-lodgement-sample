using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyCRM.Lodgement.Sample.Services.Settings;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.AddOptions();
            services.Configure<LodgementSettings>(settings => configuration.GetSection(nameof(LodgementSettings)).Bind(settings));
            services.AddHttpClient(nameof(LodgementClient), (provider, httpClient) =>
            {
                var settings = provider.GetRequiredService<IOptions<LodgementSettings>>()?.Value;

                if (string.IsNullOrWhiteSpace(settings?.Url))
                {
                    throw new ApplicationException($"No Lodgement Endpoint configured. Please ensure the app settings or environment variables contains {nameof(LodgementSettings)}_{nameof(LodgementSettings.Url)}");
                }
                httpClient.BaseAddress = new Uri(settings.Url);
            });
            services.AddTransient<ILodgementClient, LodgementClient>();
            services.AddTransient<IPackageSerializer, PackageSerializer>();
            return services;
        }
    }
}
