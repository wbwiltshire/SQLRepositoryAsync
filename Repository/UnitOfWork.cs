using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data.Interfaces;
using SQLRepositoryAsync.Data.POCO;

namespace SQLRepositoryAsync.Data.Repository
{
    public class UnitOfWork : IUOW
    {
        private readonly ILogger logger;
        private DBConnection db;
        private AppSettingsConfiguration settings;
        private string CMDText;
        private int transactionCount;

        public UnitOfWork(AppSettingsConfiguration s, ILogger l)
        {
            settings = s;
            logger = l;
            transactionCount = 0;
            db = new DBConnection(settings.Database.ConnectionString, l);
        }

        public DBConnection DBconnection { get { return db; } }
        public AppSettingsConfiguration Settings { get { return settings; } }

        public async Task<bool> Enlist() 
        {
            string CMDText = "SET XACT_ABORT ON; BEGIN TRAN T1;";
            int rows;

            transactionCount++;
            //Begin transaction, if first time
            if (transactionCount == 1)
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

        public async Task<bool> Save()
        {
            CMDText = "COMMIT TRAN T1;";
            bool status = false;
            int rows;

            //TODO: Do I need to check connection state here?
            //ANSWER: No, if we don't have an open connection, we have bigger problems
            //if (db.Connection.State != ConnectionState.Open)
            //    await db.OpenConnection();

            //Nothing to do if no transactions have enlisted
            if (transactionCount > 0)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                    {
                        rows = await cmd.ExecuteNonQueryAsync();
                        status = true;
                        transactionCount = 0;
                        logger.LogInformation("Save complete and unit of work committed.");
                    }
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex.Message);
                }
            }
            else
            {
                status = true;
                logger.LogInformation("Save ignored, because no transactions have enlisted.");
            }
            return status;
        }

        public async Task<bool> Rollback()
        {
            CMDText = "ROLLBACK TRAN T1;";
            bool status = false;
            int rows;

            //TODO: Do I need to check connection state here?
            //ANSWER: No, if we don't have an open connection, we have bigger problems
            //if (db.Connection.State != ConnectionState.Open)
            //    await db.OpenConnection();

            if (transactionCount > 0)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                    {
                        rows = await cmd.ExecuteNonQueryAsync();
                        status = true;
                        transactionCount = 0;
                        logger.LogInformation("Rollback complete and unit of work cleared.");
                    }
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex.Message);
                }
            }
            else
            {
                status = true;
                logger.LogInformation("Rollback ignored, because no transactions have enlisted.");
            }
            return status;
        }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //We'll close here, if UOW.  Otherwise, close in RepositoryBase
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (db.Connection != null)
                    {
                        logger.LogInformation("Disposing of SQL Connection from UOW");
                        db.Close();
                        db.Connection.Dispose();
                    }
                    //Nothing to do at this point;
                    transactionCount = 0;
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
