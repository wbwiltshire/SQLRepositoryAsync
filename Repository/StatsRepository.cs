using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.Interfaces;
using SQLRepositoryAsync.Data.POCO;

namespace SQLRepositoryAsync.Data.Repository
{
    public class StatsRepository : RepositoryBase<Stats>
    {
        private const string GETDASHBOARDSTATS_STMT = "";
        private const string GETDASHBOARDSTATS_PROC = "uspGetStats";

        private ILogger logger;

        #region ctor
        //Default constructor calls the base ctor
        public StatsRepository(AppSettingsConfiguration s, ILogger l, DBContext d) :
            base(s, l, d)
        { Init(l); }

        private void Init(ILogger l)
        {
            logger = l;
            OrderBy = "Id";
        }
        #endregion

        #region GetStats
        public async Task<Stats> GetStats()
        {
            string storedProcedure = String.Empty;
            Stats stats = new Stats() { Result = String.Empty };
            IList<SqlParameter> parms = new List<SqlParameter>();

            storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == GETDASHBOARDSTATS_PROC);
            if (storedProcedure == null)
            {
                SqlCommandType = Constants.DBCommandType.SQL;
                CMDText = GETDASHBOARDSTATS_STMT;
            }
            else
            {
                SqlCommandType = Constants.DBCommandType.SPROC;
                CMDText = storedProcedure;
            }
            stats.Result = await base.ExecJSONQuery(parms);
            return stats;
        }
        #endregion
    }
}