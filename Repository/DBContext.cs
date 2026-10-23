using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data.Interfaces;

namespace SQLRepositoryAsync.Data.Repository
{
    public sealed class DBContextScope : IAsyncDisposable
    {
        private readonly SqlConnection connection;

        internal DBContextScope(SqlConnection connection)
        {
            this.connection = connection;
        }

        public SqlConnection Connection => connection;

        public async ValueTask DisposeAsync()
        {
            if (connection.State != ConnectionState.Closed)
                await connection.CloseAsync();
            connection.Dispose();
        }
    }

    public class DBContext : IDBContext
    {
        private readonly ILogger logger;
        private readonly string connectionString;
        private SqlConnection connection = null;
        private bool isOpen = false;

        //ctor
        public DBContext(string connectionString, ILogger l)
        {
            logger = l;
            this.connectionString = connectionString;
            isOpen = false;
            try
            {
                connection = new SqlConnection(connectionString);
                logger.LogInformation("Database connection context created.");

            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public SqlConnection Connection
        {
            get { return connection; }
        }

        public async Task<DBContextScope> CreateScopeAsync()
        {
            SqlConnection scopedConnection = new SqlConnection(connectionString);
            await scopedConnection.OpenAsync();
            return new DBContextScope(scopedConnection);
        }

        public async Task<bool> Open()
        {
            //Only create a connection, if we don't already have one
            if (connection.State != ConnectionState.Open ) {
                try
                {
                    await connection.OpenAsync();
                    isOpen = true;
                    logger.LogInformation("DBConnection opened.");
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex.Message);
                }
            }
            else
                logger.LogInformation("Using open DBConnection.");

            return isOpen;
        }

        public void Close()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                    isOpen = false;
                    logger.LogInformation("DBConnection closed.");
                }  
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (connection != null)
                        connection.Close();
                    logger.LogInformation("DBConnection closed via dispose.");
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
