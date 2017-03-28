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

        public UnitOfWork(ILogger l)
        {
            logger = l;
            TransactionCount = 0;
        }

        public async Task<bool> Enlist(SqlConnection conn) 
        {
            string CMDText = "BEGIN TRAN T1;";
            int rows;

            TransactionCount++;
            //Begin transaction, if first time
            if (TransactionCount == 1)
            {
                try
                {

                    logger.LogInformation($"ConnectionString: {conn.ConnectionString}");

                    using (SqlCommand cmd = new SqlCommand(CMDText, conn))
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
