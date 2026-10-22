using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SQLRepositoryAsync.Data.POCO;
using SQLRepositoryAsync.Data.Repository;
using Xunit;

namespace Regression.Test
{
    public class SetupFixture : IDisposable
    {
        private ILogger logger;
        private AppSettingsConfiguration settings;
        private IConfigurationRoot config;

        public SetupFixture()
        {
            var builder = new ConfigurationBuilder().
                AddJsonFile("appsettings.json");
            config = builder.Build();
            settings = new AppSettingsConfiguration();
            ConfigurationBinder.Bind(config, settings);
            // Replace placeholders using  BuildConnectionString function
            settings.Database.ConnectionString = BuildConnectionString(config);

            ILoggerFactory logFactory = new LoggerFactory()
                .AddFile(config.GetSection("Logging"));
            logger = logFactory.CreateLogger(typeof(QueryTests));
        }

        public ILogger Logger { get { return logger; } }
        public AppSettingsConfiguration Settings { get { return settings; } }

        public static string BuildConnectionString(IConfiguration config)
        {
            var template = config.GetSection("Database")["ConnectionString"];
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            return template.Replace("${DB_PASSWORD}", password);
        }


        public void Dispose()
        {
        }
    }

    [CollectionDefinition("Test Collection")]
    public class TestCollection : ICollectionFixture<SetupFixture>
    {

    }

}
