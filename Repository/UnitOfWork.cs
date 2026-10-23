using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data.Interfaces;
using SQLRepositoryAsync.Data.POCO;

namespace SQLRepositoryAsync.Data.Repository
{
    public class UnitOfWork : IUOW
    {
        private readonly ILogger logger;
        private readonly DBContext dbc;
        private SqlTransaction transaction;

        internal SqlTransaction Transaction => transaction;

        public UnitOfWork(DBContext d, ILogger l)
        {
            logger = l;
            dbc = d;
        }

        public async Task<bool> Enlist()
        {
            if (transaction is not null)
                return true;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open && !await dbc.Open())
                    return false;

                transaction = dbc.Connection.BeginTransaction();
                logger.LogInformation("Unit of work transaction started.");
                return true;
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Unable to start unit of work transaction.");
                return false;
            }
        }

        public async Task<bool> Save()
        {
            if (transaction is null)
            {
                logger.LogInformation("Save ignored, because no transactions have enlisted.");
                return true;
            }

            try
            {
                await transaction.CommitAsync();
                transaction.Dispose();
                transaction = null;
                logger.LogInformation("Save complete and unit of work committed.");
                return true;
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Unable to commit unit of work transaction.");
                return false;
            }
        }

        public async Task<bool> Rollback()
        {
            if (transaction is null)
            {
                logger.LogInformation("Rollback ignored, because no transactions have enlisted.");
                return true;
            }

            try
            {
                await transaction.RollbackAsync();
                transaction.Dispose();
                transaction = null;
                logger.LogInformation("Rollback complete and unit of work cleared.");
                return true;
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Unable to roll back unit of work transaction.");
                return false;
            }
        }

    }
}
