using Microsoft.Extensions.Configuration;

namespace MyCRM.Lodgement.Automation.Services
{
    internal static class Configuration
    {
        private static readonly object Lock = new object();
        private static AutomationSettings _settings;

        public static AutomationSettings AutomationSettings
        {
            get
            {
                lock (Lock)
                {
                    if (_settings != null) return _settings;

                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("./appsettings.json", false)
                        .Build();

                    _settings = configuration.GetSection(nameof(AutomationSettings)).Get<AutomationSettings>();
                    return _settings;
                }
            }
        }
    }
}
