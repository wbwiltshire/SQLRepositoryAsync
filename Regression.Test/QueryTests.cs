using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.POCO;
using SQLRepositoryAsync.Data.Repository;
using Xunit;
//using Newtonsoft.Json;
using System.Text.Json;

namespace Regression.Test
{
    [Collection("Test Collection")]
    public class QueryTests
    {
        private SetupFixture fixture;
        private ILogger logger;
        private AppSettingsConfiguration settings;
        private DBContext dbc;

        private sealed class InvalidSqlRepository : RepositoryBase<State>
        {
            private readonly ILogger repositoryLogger;

            public InvalidSqlRepository(AppSettingsConfiguration settings, ILogger logger, DBContext context)
                : base(settings, logger, context)
            {
                repositoryLogger = logger;
            }

            public async Task<ICollection<State>> FindAllWithInvalidSql()
            {
                SqlCommandType = Constants.DBCommandType.SQL;
                CMDText = "SELECT * FROM TableThatDoesNotExist";
                MapToObject = new StateMapToObject(repositoryLogger);
                return await base.FindAll();
            }
        }

        //Fixture instantiated at the beginning of all the tests in this class and passed to constructor
        public QueryTests(SetupFixture f)
        {
            fixture = f;
            logger = f.Logger;
            settings = f.Settings;
        }

        #region Connection Test
        [Fact]
        public async Task ConnectionTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            Assert.True(await dbc.Open());
            Assert.True(dbc.Connection.State == ConnectionState.Open);

            dbc.Close();
        }
        #endregion

        #region FindAll Test
        [Fact]
        public void ProjectMappingFailureIsPropagatedTest()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Rows.Add(1);
            using DataTableReader reader = table.CreateDataReader();
            Assert.True(reader.Read());

            ProjectMapToObject mapper = new ProjectMapToObject(NullLogger.Instance);
            Assert.Throws<ArgumentException>(() => mapper.Execute(reader));

            DataTable projectContactTable = new DataTable();
            projectContactTable.Columns.Add("ProjectId", typeof(int));
            projectContactTable.Rows.Add(1);
            using DataTableReader projectContactReader = projectContactTable.CreateDataReader();
            Assert.True(projectContactReader.Read());

            ProjectContactMapToObject projectContactMapper =
                new ProjectContactMapToObject(NullLogger.Instance);
            Assert.Throws<ArgumentException>(() => projectContactMapper.Execute(projectContactReader));
        }

        [Fact]
        public async Task SqlFailureIsPropagatedTest()
        {
            using (dbc = new DBContext(settings.Database.ConnectionString, logger))
            {
                InvalidSqlRepository repos = new InvalidSqlRepository(settings, logger, dbc);

                await Assert.ThrowsAsync<SqlException>(() => repos.FindAllWithInvalidSql());
            }
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
        #endregion

        #region FindAll Paged Test with default Sort
        [Fact]
        public async Task ConcurrentSharedRepositoryOperationsTest()
        {
            using (dbc = new DBContext(settings.Database.ConnectionString, logger))
            {
                Assert.True(await dbc.Open());
                ContactRepository repos = new ContactRepository(settings, logger, dbc);

                Task<ICollection<Contact>> allContactsTask = repos.FindAll();
                Task<Contact> singleContactTask = repos.FindByPK(new PrimaryKey() { Key = 1 });
                ICollection<Contact> allContacts = await allContactsTask;
                Contact singleContact = await singleContactTask;

                Assert.NotEmpty(allContacts);
                Assert.NotNull(singleContact);
                Assert.Equal(1, singleContact.Id);
            }
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
            IPager<State> states = await stateRepos.FindAll(new Pager<State>()
            {
                PageNbr = 2,
                PageSize = 5,
                SortColumn = "Id",
                Direction = SQLOrderBy.DESC
            });
            Assert.NotNull(states.Entities);
            Assert.True(states.RowCount > 0);
            Assert.NotNull(states.Entities);
            Assert.True(states.Entities.Select(state => state.Id).SequenceEqual(
                states.Entities.Select(state => state.Id).OrderByDescending(id => id)));

            dbc.Close();
        }

        [Fact]
        public async Task FindAllPagedDirectSqlTest()
        {
            AppSettingsConfiguration directSqlSettings = new AppSettingsConfiguration()
            {
                Database = new Database()
                {
                    ConnectionString = settings.Database.ConnectionString,
                    StoredProcedures = settings.Database.StoredProcedures
                        .Where(procedure => procedure != "uspFindAllCityPaged"
                            && procedure != "uspFindAllProjectPaged"
                            && procedure != "uspFindAllProjectContactPaged")
                        .ToArray()
                }
            };

            using (dbc = new DBContext(directSqlSettings.Database.ConnectionString, logger))
            {
                CityRepository cityRepos = new CityRepository(directSqlSettings, logger, dbc);
                IPager<City> cities = await cityRepos.FindAll(new Pager<City>()
                {
                    PageNbr = 1,
                    PageSize = 5,
                    SortColumn = "Id",
                    Direction = SQLOrderBy.DESC
                });

                Assert.Equal(5, cities.Entities.Count);
                Assert.True(cities.RowCount > cities.Entities.Count);
                Assert.True(cities.Entities.Select(city => city.Id).SequenceEqual(
                    cities.Entities.Select(city => city.Id).OrderByDescending(id => id)));

                ProjectRepository projectRepos = new ProjectRepository(directSqlSettings, logger, dbc);
                IPager<Project> projects = await projectRepos.FindAll(new Pager<Project>()
                {
                    PageNbr = 0,
                    PageSize = 5,
                    SortColumn = "Id",
                    Direction = SQLOrderBy.DESC
                });

                Assert.True(projects.Entities.Count > 0);
                Assert.Equal(projects.RowCount, projects.Entities.Count);
                Assert.True(projects.Entities.Select(project => project.Id).SequenceEqual(
                    projects.Entities.Select(project => project.Id).OrderByDescending(id => id)));

                ProjectContactRepository projectContactRepos =
                    new ProjectContactRepository(directSqlSettings, logger, dbc);
                IPager<ProjectContact> projectContacts = await projectContactRepos.FindAll(new Pager<ProjectContact>()
                {
                    PageNbr = 0,
                    PageSize = 5,
                    SortColumn = "ProjectId",
                    Direction = SQLOrderBy.DESC
                });

                Assert.True(projectContacts.Entities.Count > 0);
                Assert.Equal(projectContacts.RowCount, projectContacts.Entities.Count);
                Assert.True(projectContacts.Entities.Select(projectContact => projectContact.ProjectId).SequenceEqual(
                    projectContacts.Entities.Select(projectContact => projectContact.ProjectId).OrderByDescending(id => id)));
            }
        }

        [Fact]
        public async Task UnsupportedProjectViewsFailExplicitly()
        {
            using DBContext context = new DBContext(settings.Database.ConnectionString, logger);
            ProjectRepository projectRepos = new ProjectRepository(settings, logger, context);
            ProjectContactRepository projectContactRepos =
                new ProjectContactRepository(settings, logger, context);

            await Assert.ThrowsAsync<NotSupportedException>(() => projectRepos.FindAllView());
            await Assert.ThrowsAsync<NotSupportedException>(() => projectRepos.FindAllView(new Pager<Project>()));
            await Assert.ThrowsAsync<NotSupportedException>(() => projectRepos.FindByPKView(new PrimaryKey()));
            await Assert.ThrowsAsync<NotSupportedException>(() => projectContactRepos.FindAllView());
            await Assert.ThrowsAsync<NotSupportedException>(() => projectContactRepos.FindAllView(new Pager<ProjectContact>()));
            await Assert.ThrowsAsync<NotSupportedException>(() => projectContactRepos.FindByPKView(new PrimaryKey()));
        }
        #endregion

        #region FindAll View Test
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
        #endregion

        #region FindAll View Paged Test
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
            IPager<City> cities = await cityRepos.FindAllView(new Pager<City>()
            {
                PageNbr = 2,
                PageSize = 5,
                SortColumn = "Id",
                Direction = SQLOrderBy.DESC
            });
            Assert.NotEmpty(cities.Entities);
            Assert.True(cities.RowCount > 0);
            Assert.NotNull(cities.Entities);
            Assert.NotNull(cities.Entities.FirstOrDefault().State);
            Assert.True(cities.Entities.Select(city => city.Id).SequenceEqual(
                cities.Entities.Select(city => city.Id).OrderByDescending(id => id)),
                "City IDs were: " + String.Join(",", cities.Entities.Select(city => city.Id)));

            dbc.Close();
        }
        #endregion

        #region Find By PK (Alpha) Test
        [Fact]
        public async Task FindByPKAlphaTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            StateRepository repos = new StateRepository(settings, logger, dbc);

            State state = await repos.FindByPK(new PrimaryKey() { Key = "FL" });
            Assert.NotNull(state);

            dbc.Close();
        }
        #endregion

        #region Find By PK (Numeric) Test
        [Fact]
        public async Task FindByPKNumericTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository repos = new ContactRepository(settings, logger, dbc);

            Contact contact = await repos.FindByPK(new PrimaryKey() { Key =1 });
            Assert.NotNull(contact);
        }
        #endregion

        #region Find By Composite Key Test
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
        #endregion 

        #region FindView By PK (Numeric) Test 
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
        #endregion

        #region FindAll Paged with Sort Test
        [Fact]
        public async Task FindAllPageSorted()
        {
            IPager<Contact> pager;
            int page = 0;
            int pageSize = 10;

            using (dbc = new DBContext(settings.Database.ConnectionString, logger))
            {
                Assert.NotNull(dbc);
                ContactRepository repos = new ContactRepository(settings, logger, dbc);
                pager = await repos.FindAll(new Pager<Contact>() { PageNbr = page, PageSize =pageSize, SortColumn = "LastName", Direction = SQLOrderBy.DESC });
                Assert.True(pager.RowCount > 0);
                Assert.True(pager.Entities.Count == pageSize);
                Assert.True(pager.Entities.FirstOrDefault().LastName.Trim() == "Vader");
                // TODO: Need additional tests / asserts
            }

        }
        #endregion

        #region FindAll Paged with Filter Test
        [Fact]
        public async Task FindAllPageFiltered()
        {
            IPager<Contact> pager;
            int page = 0;
            int pageSize = 10;
            string firstColName = "LastName";
            string firstFilterValue = "Vader";
            string secondColName = "CityId";
            int secondFilterValue = 16;
            string dateColName = "CreateUtcDt";
            string dateFilterValue = "2017-02-15 00:00:00.000";

            using (dbc = new DBContext(settings.Database.ConnectionString, logger))
            {
                Assert.NotNull(dbc);
                ContactRepository repos = new ContactRepository(settings, logger, dbc);
                pager = await repos.FindAllFiltered(new Pager<Contact>() { PageNbr = page, PageSize = pageSize, FilterColumn = firstColName, FilterValue = firstFilterValue });
                Assert.True(pager.RowCount > 0);
                Assert.NotNull(pager.Entities);
                Assert.True(pager.Entities.Count == 1);
                Assert.True(pager.Entities.FirstOrDefault().LastName.Trim() == firstFilterValue);
                pager = await repos.FindAllFiltered(new Pager<Contact>() { PageNbr = page, PageSize = pageSize, FilterColumn = secondColName, FilterValue = secondFilterValue });
                Assert.True(pager.RowCount > 0);
                Assert.NotNull(pager.Entities);
                Assert.True(pager.Entities.Count == 4);
                Assert.True(pager.Entities.FirstOrDefault().CityId == secondFilterValue);
                pager = await repos.FindAllFiltered(new Pager<Contact>() { PageNbr = page, PageSize = pageSize, FilterColumn = dateColName, FilterValue = dateFilterValue });
                Assert.True(pager.RowCount > 0);
                Assert.NotNull(pager.Entities);
                Assert.True(pager.Entities.Count == 2);
                Assert.True(pager.Entities.FirstOrDefault().CreateUtcDt.ToString("yyyy-MM-dd HH:mm:ss.fff") == dateFilterValue);
                // TODO: Need additional tests / asserts
            }

        }
        #endregion

        #region Execute NonQuery Test
        [Fact]
        public async Task ExecNonQueryTest()
        {
            Assert.NotNull(dbc = new DBContext(settings.Database.ConnectionString, logger));
            ContactRepository repos = new ContactRepository(settings, logger, dbc);

            Assert.True(await repos.NonQuery() > 1);

            dbc.Close();
        }
        #endregion

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
            //statsView = JsonConvert.DeserializeObject<StatsView>(stats.Result);
            statsView = JsonSerializer.Deserialize<StatsView>(stats.Result);
            Assert.NotNull(statsView);
            Assert.True(statsView.TotalContacts > 0);
            dbc.Close();
        }

    }
}
