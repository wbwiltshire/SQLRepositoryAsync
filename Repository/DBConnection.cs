using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SQLRepositoryAsync.Data.Repository
{
    public sealed class DBConnection
    {
        private ILogger logger;
        private SqlConnection connection = null;


        //ctor
        public DBConnection(string connectionString, ILogger l)
        {
            logger = l;
            try
            {
                connection = new SqlConnection(connectionString);
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

        internal async Task OpenConnection()
        {
            //Only create a connection, if we don't already have one
            if (connection.State != ConnectionState.Open ) {
                try
                {
                    await connection.OpenAsync();
                    logger.LogInformation("Repository connection opened.");
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex.Message);
                }
            }
            else
                logger.LogInformation("Repository connection already open.");
        }

        internal void Close()
        {
            try
            {
                if (connection != null)
                    connection.Close();
                logger.LogInformation("Repository connection closed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
