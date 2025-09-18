using Microsoft.Extensions.Configuration;
using System.IO;

namespace FionaAutomation.Utils
{
    public static class ConfigReader
    {
        private static IConfigurationRoot config;

        static ConfigReader()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static IConfigurationRoot GetConfig()
        {
            return config;
        }
    }
}
