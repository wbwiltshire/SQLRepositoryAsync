using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.POCO;
using SQLRepositoryAsync.Data.Repository;
using Xunit;
using Newtonsoft.Json;

namespace Regression.Test
{
    [Collection("Test Collection")]
    public class QueryTests
    {
        private SetupFixture fixture;
        private ILogger logger;
        private AppSettingsConfiguration settings;
        private DBContext dbc;

        //Fixture instantiated at the beginning of all the tests in this class and passed to constructor
        public QueryTests(SetupFixture f)
        {
            fixture = f;
            logger = f.Logger;
            settings = f.Settings;
        }

        [Fact]
        public async Task ConnectionTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            Assert.True(await dbc.Open());
            Assert.True(dbc.Connection.State == ConnectionState.Open);

            dbc.Close();
        }

        [Fact]
        public async Task FindAllTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository contactRepos = new ContactRepository(settings, logger, dbc);
            CityRepository cityRepos = new CityRepository(settings, logger, dbc);
            StateRepository stateRepos = new StateRepository(settings, logger, dbc);

            ICollection<Contact> contacts = await contactRepos.FindAll();
            Assert.NotEmpty(contacts);
            ICollection<City> cities = await cityRepos.FindAll();
            Assert.NotEmpty(cities);
            ICollection<State> states = await stateRepos.FindAll();
            Assert.NotEmpty(states);

            dbc.Close();
        }

        [Fact]
        public async Task FindAllPagedTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository contactRepos = new ContactRepository(settings, logger, dbc);
            CityRepository cityRepos = new CityRepository(settings, logger, dbc);
            StateRepository stateRepos = new StateRepository(settings, logger, dbc);

            IPager<Contact> contacts = await contactRepos.FindAll(new Pager<Contact>() { PageNbr = 2, PageSize = 5 });
            Assert.NotNull(contacts);
            Assert.True(contacts.RowCount > 0);
            Assert.NotNull(contacts.Entities);
            IPager<City> cities = await cityRepos.FindAll(new Pager<City>() { PageNbr = 2, PageSize = 5 });
            Assert.NotNull(cities.Entities);
            Assert.True(cities.RowCount > 0);
            Assert.NotNull(cities.Entities);
            IPager<State> states = await stateRepos.FindAll(new Pager<State>() { PageNbr = 2, PageSize = 5 });
            Assert.NotNull(states.Entities);
            Assert.True(states.RowCount > 0);
            Assert.NotNull(states.Entities);

            dbc.Close();
        }

        [Fact]
        public async Task FindAllViewTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository contactRepos = new ContactRepository(settings, logger, dbc);
            CityRepository cityRepos = new CityRepository(settings, logger, dbc);

            ICollection<Contact> contacts = await contactRepos.FindAllView();
            Assert.NotEmpty(contacts);
            Assert.NotNull(contacts.FirstOrDefault().City);
            Assert.NotNull(contacts.FirstOrDefault().City.State);
            ICollection<City> cities = await cityRepos.FindAllView();
            Assert.NotEmpty(cities);
            Assert.NotNull(cities.FirstOrDefault().State);

            dbc.Close();
        }

        [Fact]
        public async Task FindAllViewPagedTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository contactRepos = new ContactRepository(settings, logger, dbc);
            CityRepository cityRepos = new CityRepository(settings, logger, dbc);

            IPager<Contact> contacts = await contactRepos.FindAllView(new Pager<Contact>() { PageNbr = 2, PageSize = 5 });
            Assert.NotEmpty(contacts.Entities);
            Assert.True(contacts.RowCount > 0);
            Assert.NotNull(contacts.Entities);
            Assert.NotNull(contacts.Entities.FirstOrDefault().City);
            Assert.NotNull(contacts.Entities.FirstOrDefault().City.State);
            IPager<City> cities = await cityRepos.FindAllView(new Pager<City>() { PageNbr = 2, PageSize = 5 });
            Assert.NotEmpty(cities.Entities);
            Assert.True(cities.RowCount > 0);
            Assert.NotNull(cities.Entities);
            Assert.NotNull(cities.Entities.FirstOrDefault().State);

            dbc.Close();
        }

        [Fact]
        public async Task FindByPKAlphaTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            StateRepository repos = new StateRepository(settings, logger, dbc);

            State state = await repos.FindByPK(new PrimaryKey() { Key = "FL" });
            Assert.NotNull(state);

            dbc.Close();
        }

        [Fact]
        public async Task FindByPKNumericTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository repos = new ContactRepository(settings, logger, dbc);

            Contact contact = await repos.FindByPK(new PrimaryKey() { Key =1 });
            Assert.NotNull(contact);
        }

        [Fact]
        public async Task FindByCompositeKeyTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ProjectContactRepository repos = new ProjectContactRepository(settings, logger, dbc);
            int projectId = 1;
            int contactId = 2;

            ProjectContact pc = await repos.FindByPK(new PrimaryKey() { CompositeKey = new object[] { projectId, contactId }, IsComposite = true });
            Assert.NotNull(pc);
        }

        [Fact]
        public async Task FindViewByPKTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository repos = new ContactRepository(settings, logger, dbc);

            Contact contact = await repos.FindViewByPK(new PrimaryKey() { Key = 1 });
            Assert.NotNull(contact);
            Assert.True(contact.Id == 1);
            Assert.NotNull(contact.City);
            Assert.NotNull(contact.City.State);

            dbc.Close();
        }

        [Fact]
        public async Task ExecNonQueryTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository repos = new ContactRepository(settings, logger, dbc);

            Assert.True(await repos.NonQuery() > 1);

            dbc.Close();
        }

        [Fact]
        public async Task ExecStoredProcTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository contactRepos = new ContactRepository(settings, logger, dbc);

            Assert.True(await contactRepos.StoredProc(1) == 1);

            dbc.Close();
        }

        [Fact]
        public async Task ExecJSONQueryTest()
        {
            Stats stats = null;
            StatsView statsView = null;

            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            StatsRepository statsRepos = new StatsRepository(settings, logger, dbc);

            stats = await statsRepos.GetStats();
            statsView = JsonConvert.DeserializeObject<StatsView>(stats.Result);
            Assert.NotNull(statsView);
            Assert.True(statsView.TotalContacts > 0);
            dbc.Close();
        }

    }
}
