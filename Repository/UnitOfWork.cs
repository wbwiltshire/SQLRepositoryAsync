using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.POCO;

namespace SQLRepositoryAsync.Data.Repository
{
    public class UnitOfWork
    {
        private readonly ILogger logger;
        private DBConnection db;
        private AppSettingsConfiguration settings;

        public UnitOfWork(AppSettingsConfiguration s, ILogger l)
        {
            settings = s;
            logger = l;
            TransactionCount = 0;
            db = new DBConnection(settings.Database.ConnectionString, l);
        }

        public DBConnection DBconnection { get { return db; } }
        public AppSettingsConfiguration Settings { get { return settings; } }

        public async Task<bool> Enlist() 
        {
            string CMDText = "BEGIN TRAN T1;";
            int rows;

            TransactionCount++;
            //Begin transaction, if first time
            if (TransactionCount == 1)
            {
                try
                {
                    if (db.Connection.State != ConnectionState.Open)
                        await db.OpenConnection();

                    logger.LogInformation($"ConnectionString: {Settings.Database.ConnectionString}");

                    using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                    {
                        rows = await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex.Message);
                }

            }
            return true;
        }
        public int TransactionCount { get; set; }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //We'll close here, if UOW.  Otherwise, close in RepositoryBase
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Nothing to do at this point;
                    TransactionCount = 0;
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
