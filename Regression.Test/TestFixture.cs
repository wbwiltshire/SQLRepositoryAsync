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

            ILoggerFactory logFactory = new LoggerFactory()
                .AddFile(config.GetSection("Logging"));
            logger = logFactory.CreateLogger(typeof(QueryTests));
        }

        public ILogger Logger { get { return logger; } }
        public AppSettingsConfiguration Settings { get { return settings; } }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("Test Collection")]
    public class TestCollection : ICollectionFixture<SetupFixture>
    {

    }
}
