/******************************************************************************************************
 *This class was generated on 04/20/2014 09:31:37 using Repository Builder version 0.9. *
 *The class was generated from Database: BACS and Table: State.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.Interfaces;
using SQLRepositoryAsync.Data.POCO;

namespace SQLRepositoryAsync.Data.Repository
{

    public class StateRepository : RepositoryBase<State>, IRepository<State>
    {
        private const string FINDALLCOUNT_STMT = "SELECT COUNT(Id) FROM State WHERE Active=1"; 
        private const string FINDALL_STMT = "SELECT Id,Name,Active,ModifiedUtcDt,CreateUtcDt FROM State WHERE Active=1";
        //private const string FINDALLPAGER_STMT = "SELECT TOP({0}) Id, Name, Active, ModifiedUtcDt, CreateUtcDt FROM (SELECT Id, Name, Active, ModifiedUtcDt, CreateUtcDt, ROW_NUMBER() OVER (ORDER BY {1}) AS [rc] FROM State) AS s WHERE rc > {2}";
        private const string FINDALLPAGER_STMT = "SELECT Id,Name,Active,ModifiedUtcDt,CreateUtcDt FROM State WHERE Active=1 @orderBy OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
        private const string FINDBYPK_STMT = "SELECT Id, Name, Active, ModifiedUtcDt, CreateUtcDt FROM State WHERE Id =@pk AND Active=1";
        private const string ADD_STMT = "INSERT INTO State (Id, Name, Active, ModifiedUtcDt, CreateUtcDt) VALUES (@pk, @p1, 1, GETDATE(), GETDATE())";
        private const string UPDATE_STMT = "UPDATE State SET Name=@p1, ModifiedUtcDt=GETDATE() WHERE Id =@pk AND Active=1";
        private const string DELETE_STMT = "UPDATE State SET Active=0, ModifiedUtcDt=GETDATE() WHERE Id =@pk";
        private const string ORDERBY_STMT = " ORDER BY ";
        private const string ADD_PROC = "uspAddState";
        private const string UPDATE_PROC = "uspUpdateState";

        private ILogger logger;

        #region ctor
        //Default constructor calls the base ctor
        public StateRepository(AppSettingsConfiguration s, ILogger l, DBContext d) :
            base(s, l, d)
        { Init(l); }
        public StateRepository(AppSettingsConfiguration s, ILogger l, UnitOfWork uow, DBContext d) :
            base(s, l, uow, d)
        { Init(l); }

        private void Init(ILogger l)
        {
            logger = l;
            // Set default ordering
            OrderByColumns = new Dictionary<string, int>() { { "Id", 1 } };
            AddOrderByStatement("Id", SQLOrderBy.ASC);
        }
        #endregion

        #region FindAll
        public override async Task<ICollection<State>> FindAll()
        {
            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = BuildCommandText(FINDALL_STMT);
            MapToObject = new StateMapToObject(logger);
            return await base.FindAll();
        }
        #endregion

        #region FindAll(Pager)
        public async Task<IPager<State>> FindAll(IPager<State> pager)
        {
            IList<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@offset", pager.PageSize * pager.PageNbr));
            parms.Add(new SqlParameter("@pageSize", pager.PageSize));

            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = BuildCommandText(FINDALLPAGER_STMT);
            MapToObject = new StateMapToObject(logger);
            pager.Entities = await base.FindAll(parms);

            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }
        #endregion

        #region FindAllFiltered(Pager)
        public async Task<IPager<State>> FindAllFiltered(IPager<State> pager)
        {
            await Task.Run( () => { throw new NotImplementedException(); } );
            return pager;
        }
        #endregion

        #region FindByPK(IPrimaryKey pk)
        public override async Task<State> FindByPK(IPrimaryKey pk)
        {
            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = FINDBYPK_STMT;
            MapToObject = new StateMapToObject(logger);
            return await base.FindByPK(pk);
        }
        #endregion

        #region Add
        public async Task<object> Add(State entity)
        {
            string storedProcedure = String.Empty;
            object result;
            int rows;

            storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == ADD_PROC);
            if (storedProcedure == null)
            {
                SqlCommandType = Constants.DBCommandType.SQL;
                CMDText = ADD_STMT;
            }
            else
            {
                SqlCommandType = Constants.DBCommandType.SPROC;
                CMDText = storedProcedure;
            }
            MapFromObject = new StateMapFromObject(logger);
            result = await base.Add(entity, entity.PK);
            if (result != null)
                rows = (int)result;
            else
                rows = -1;
            return rows;
        }
        #endregion  

        #region Update
        public async Task<int> Update(State entity)
        {
            string storedProcedure = String.Empty;

            storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == UPDATE_PROC);
            if (storedProcedure == null)
            {
                SqlCommandType = Constants.DBCommandType.SQL;
                CMDText = UPDATE_STMT;
            }
            else
            {
                SqlCommandType = Constants.DBCommandType.SPROC;
                CMDText = storedProcedure;
            }
            MapFromObject = new StateMapFromObject(logger);
            return await base.Update(entity, entity.PK);
        }
        #endregion

        #region Delete
        public async Task<int> Delete(PrimaryKey pk)
        {
            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = DELETE_STMT;
            return await base.Delete(pk);
        }
        #endregion
    }
}


