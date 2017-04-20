﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.POCO;
using SQLRepositoryAsync.Data.Repository;
using Xunit;

namespace Regression.Test
{
    public class TransactionTests
    {
        private ILogger logger;
        private AppSettingsConfiguration settings;
        private IConfigurationRoot config;

        public TransactionTests()
        {
            ILoggerFactory logFactory = new LoggerFactory()
                .AddDebug();
            logger = logFactory.CreateLogger(typeof(TransactionTests));

            var builder = new ConfigurationBuilder().
                AddJsonFile("appsettings.json");
            config = builder.Build();
            settings = new AppSettingsConfiguration();
            ConfigurationBinder.Bind(config, settings);
        }

        [Fact]
        public async Task UOWConnectionTest()
        {
            UnitOfWork uow = new UnitOfWork(settings, logger);
            StateRepository repos = new StateRepository(logger, uow);
            Assert.True(await repos.Ping());
        }

        [Fact]
        public async Task UpdateTest()
        {
            ContactRepository contactRepos = new ContactRepository(settings, logger);
            CityRepository cityRepos = new CityRepository(settings, logger);
            StateRepository stateRepos = new StateRepository(settings, logger);
            string oldString = String.Empty;
            string updateString = String.Empty;
            int rows = 0;

            #region Update Contact Test
            oldString = "No notes";
            updateString = "Updated note.";
            Contact contact = await contactRepos.FindByPK(new PrimaryKey() { Key = 1 });
            Assert.NotNull(contact);
            Assert.Equal(contact.Notes, oldString);
            contact.Notes = updateString;
            rows = await contactRepos.Update(contact);
            Assert.Equal(rows, 1);
            contact = await contactRepos.FindByPK(new PrimaryKey() { Key = 1 });
            Assert.Equal(contact.Notes, updateString);
            #endregion

            #region Update City Test
            oldString = "Tampa";
            updateString = "Tampa(Updated)";
            City city = await cityRepos.FindByPK(new PrimaryKey() { Key = 1 });
            Assert.NotNull(city);
            Assert.Equal(city.Name, oldString);
            city.Name = updateString;
            rows = await cityRepos.Update(city);
            Assert.Equal(rows, 1);
            city = await cityRepos.FindByPK(new PrimaryKey() { Key = 1 });
            Assert.Equal(city.Name, updateString);
            #endregion

            #region Update State Test
            oldString = "NA";
            updateString = "NA(Updated)";
            State state = await stateRepos.FindByPK(new PrimaryKey() { Key = "00" });
            Assert.NotNull(state);
            Assert.Equal(state.Name, oldString);
            state.Name = updateString;
            rows = await stateRepos.Update(state);
            Assert.Equal(rows, 1);
            state = await stateRepos.FindByPK(new PrimaryKey() { Key = "00" });
            Assert.Equal(state.Name, updateString);
            #endregion

        }

        [Fact]
        public async Task AddTest()
        {
            ContactRepository contactRepos = new ContactRepository(settings, logger);
            CityRepository cityRepos = new CityRepository(settings, logger);
            StateRepository stateRepos = new StateRepository(settings, logger);
            string skey = string.Empty;
            int key = 0;

            #region Add Contact Test
            Contact contact = new Contact()
            {
                FirstName = "New",
                LastName = "User",
                Address1 = "Address1",
                Address2 = "Address2",
                CellPhone = "8005551212",
                HomePhone = "8005551212",
                WorkPhone = "8005551212",
                Notes = String.Empty,
                ZipCode = "99999",              
                EMail = "NewUser@Mail.com",
                CityId = 1
            };
            ICollection<Contact> contacts = await contactRepos.FindAll();
            Assert.Null(contacts.Where(c => c.LastName == contact.LastName && c.FirstName == contact.FirstName).FirstOrDefault());
            key = (int) await contactRepos.Add(contact);
            Assert.True(key > 0);
            Assert.NotNull(await contactRepos.FindByPK(new PrimaryKey() { Key = key }));
            #endregion

            #region Add City Test
            City city = new City()
            {
                Name = "New City",
                StateId = "FL"
            };


            ICollection<City> cities = await cityRepos.FindAll();
            Assert.Null(cities.Where(c => c.Name == city.Name).FirstOrDefault());
            key = (int)await cityRepos.Add(city);
            Assert.True(key > 0);
            Assert.NotNull(await cityRepos.FindByPK(new PrimaryKey() { Key = key }));
            #endregion

            #region Add State Test
            State newState = new State()
            {
                Id = "ZZ",
                Name = "New State"
            };

            State state = await stateRepos.FindByPK(new PrimaryKey() { Key = newState.Id });
            Assert.Null(state);
            skey = (string)await stateRepos.Add(newState);
            Assert.True(skey == newState.Id);
            Assert.NotNull(await stateRepos.FindByPK(new PrimaryKey() { Key = newState.Id }));
            #endregion
        }

        [Fact]
        public async Task DeleteTest()
        {
            ContactRepository contactRepos = new ContactRepository(settings, logger);
            CityRepository cityRepos = new CityRepository(settings, logger);
            StateRepository stateRepos = new StateRepository(settings, logger);
            int rows = 0;

            #region Delete Contact Test
            Contact contact = await contactRepos.FindByPK(new PrimaryKey() { Key = 8 });
            Assert.NotNull(contact);
            rows = await contactRepos.Delete(new PrimaryKey() { Key = 8 });
            Assert.Equal(rows, 1);
            contact = await contactRepos.FindByPK(new PrimaryKey() { Key = 8 });
            Assert.Null(contact);
            #endregion

            #region Delete City Test
            City city = await cityRepos.FindByPK(new PrimaryKey() { Key = 17 });
            Assert.NotNull(city);
            rows = await cityRepos.Delete(new PrimaryKey() { Key = 17 });
            Assert.Equal(rows, 1);
            city = await cityRepos.FindByPK(new PrimaryKey() { Key = 17 });
            Assert.Null(city);
            #endregion

            #region Delete State Test
            State state = await stateRepos.FindByPK(new PrimaryKey() { Key = "WA" });
            Assert.NotNull(state);
            rows = await stateRepos.Delete(new PrimaryKey() { Key = "WA" });
            Assert.Equal(rows, 1);
            state = await stateRepos.FindByPK(new PrimaryKey() { Key = "WA" });
            Assert.Null(state);
            #endregion
        }

        [Fact]
        public async Task SaveTest()
        {
            string updateString = "Save this update.";
            Contact newContact = new Contact()
            {
                FirstName = "New",
                LastName = "SaveUser",
                Address1 = "Address1",
                Address2 = "Address2",
                CellPhone = "8005551212",
                HomePhone = "8005551212",
                WorkPhone = "8005551212",
                Notes = String.Empty,
                ZipCode = "99999",
                EMail = "NewSaveUser@Mail.com",
                CityId = 1
            };

            UnitOfWork uow = new UnitOfWork(settings, logger);
            ContactRepository repos = new ContactRepository(logger, uow);
            Contact contact = await repos.FindByPK(new PrimaryKey() { Key = 11 });
            contact.Notes = updateString;
            int rows = await repos.Update(contact);
            Assert.Equal(rows, 1);
            ICollection<Contact> contacts = await repos.FindAll();
            Assert.Null(contacts.Where(c => c.LastName == newContact.LastName && c.FirstName == newContact.FirstName).FirstOrDefault());
            int key = (int)await repos.Add(newContact);
            await repos.Save();
            contact = await repos.FindByPK(new PrimaryKey() { Key = 11 });
            Assert.Equal(contact.Notes, updateString);
            Assert.True(key > 0);
            Assert.NotNull(await repos.FindByPK(new PrimaryKey() { Key = key }));
        }

        [Fact]
        public async Task RollBackTest()
        {
            string updateString = "Rollback this update.";
            string oldNotes = String.Empty;

            UnitOfWork uow = new UnitOfWork(settings, logger);
            ContactRepository repos = new ContactRepository(logger, uow);
            Contact contact = await repos.FindByPK(new PrimaryKey() { Key = 11 });
            oldNotes = contact.Notes;
            contact.Notes = updateString;
            int rows = await repos.Update(contact);
            Assert.Equal(rows, 1);
            await repos.Rollback();
            contact = await repos.FindByPK(new PrimaryKey() { Key = 11 });
            Assert.Equal(contact.Notes, oldNotes);
        }
    }
}
