using LMGTech.DotNetLixi;
using Microsoft.CSharp.RuntimeBinder;
using MyCRM.Lodgement.Common.Utilities;

namespace MyCRM.Lodgement.Sample.Services.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.AddOptions();
            services.Configure<LodgementSettings>(settings =>
            {
                var lodgementSettingsSection = configuration.GetSection(nameof(LodgementSettings));
                lodgementSettingsSection.Bind(settings);
                settings.LixiPackageVersion = EnumHelper.ConvertToEnum<LixiVersion>(lodgementSettingsSection["Version"]);
            });
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
            return services;
        }

        public static IServiceCollection AddServicesSample(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            services.AddScoped<ILixiPackageService, LixiPackageService>();
            services.AddClient(configuration);
            
            return services;
        }
    }
}
