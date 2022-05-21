/******************************************************************************************************
 *This class was generated on 04/30/2014 09:06:10 using Repository Builder version 0.9. *
 *The class was generated from Database: Customer and Table: City.  *
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

    public class CityRepository : RepositoryBase<City>, IRepository<City>
    {
        private const string FINDALLCOUNT_STMT = "SELECT COUNT(Id) FROM City WHERE Active=1";
        private const string FINDALL_STMT = "SELECT Id,Name,StateId,Active,ModifiedUtcDt,CreateUtcDt FROM City WHERE Active=1";
        private const string FINDALLPAGER_STMT = "SELECT Id,Name,StateId,Active,ModifiedUtcDt,CreateUtcDt FROM City WHERE Active=1 @OrderBy OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
        private const string FINDALLVIEW_STMT = "SELECT Id,Name,StateId,StateName,Active,ModifiedUtcDt,CreateUtcDt FROM vwFindAllCityView ORDER BY Id";
        private const string FINDALLVIEWPAGER_STMT = "SELECT Id,Name,StateId,StateName,Active,ModifiedUtcDt,CreateUtcDt FROM vwFindAllCityView @orderBy OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
        private const string FINDBYPK_STMT = "SELECT Id, Name, StateId, Active, ModifiedUtcDt, CreateUtcDt FROM City WHERE Id =@pk AND Active=1";
        private const string FINDBYPKVIEW_STMT = "SELECT Id,Name,StateId,StateName,Active,ModifiedUtcDt,CreateUtcDt FROM vwFindAllCityView WHERE Id =@pk AND Active=1";
        private const string ADD_STMT = "INSERT INTO City (Name, StateId, Active, ModifiedUtcDt, CreateUtcDt) VALUES (@p1, @p2, 1, GETDATE(), GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT)";
        private const string UPDATE_STMT = "UPDATE City SET Name=@p1, StateId=@p2, Active=1, ModifiedUtcDt=GETDATE() WHERE Id =@pk AND Active=1";
        private const string DELETE_STMT = "UPDATE City SET Active=0, ModifiedUtcDt=GETDATE() WHERE Id =@pk";
        private const string ORDERBY_STMT = " ORDER BY ";
        private const string FINDALL_PAGEDPROC = "uspFindAllCityPaged";
        private const string FINDALL_PAGEDVIEWPROC = "uspFindAllCityViewPaged";
        private const string ADD_PROC = "uspAddCity";
        private const string UPDATE_PROC = "uspUpdateCity";

        private ILogger logger;

        #region ctor
        //Default constructor calls the base ctor
        public CityRepository(AppSettingsConfiguration s, ILogger l, DBContext d) :
            base(s, l, d)
        { Init(l); }
        public CityRepository(AppSettingsConfiguration s, ILogger l, UnitOfWork uow, DBContext d) :
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
        public override async Task<ICollection<City>> FindAll()
        {
            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = BuildCommandText(FINDALL_STMT);
            MapToObject = new CityMapToObject(logger);
            return await base.FindAll();
        }
        #endregion

        #region FindAll(Pager)
        public async Task<IPager<City>> FindAll(IPager<City> pager)
        {
            string storedProcedure = String.Empty;
            List<SqlParameter> parms = new List<SqlParameter>();
            MapToObject = new CityMapToObject(logger);
            parms.Add(new SqlParameter("@offset", pager.PageSize * pager.PageNbr));
            parms.Add(new SqlParameter("@pageSize", pager.PageSize));

            storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == FINDALL_PAGEDPROC);
            if (storedProcedure == null)
            {
                AddOrderByStatement(pager.SortColumn, pager.Direction);
                SqlCommandType = Constants.DBCommandType.SQL;
                CMDText = BuildCommandText(FINDALLPAGER_STMT);
                pager.Entities = await base.FindAll();
            }
            else
            {
                SqlCommandType = Constants.DBCommandType.SPROC;
                parms.AddRange(AddOrderByParmeters(pager.SortColumn, pager.Direction));
                CMDText = storedProcedure;
                pager.Entities = await base.FindAll(parms);
            }

            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }
        #endregion

        #region FindAllFiltered(Pager)
        public async Task<IPager<City>> FindAllFiltered(IPager<City> pager)
        {
            await Task.Run(() => { throw new NotImplementedException(); });
            return pager;
        }
        #endregion

        #region FindAllView
        public async Task<ICollection<City>> FindAllView()
        {
            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = BuildCommandText(FINDALLVIEW_STMT);
            MapToObject = new CityMapToObjectView(logger);
            return await base.FindAll();
        }
        #endregion

        #region FindAllView(Pager)
        public async Task<IPager<City>> FindAllView(IPager<City> pager)
        {
            string storedProcedure = String.Empty;
            IList<SqlParameter> parms = new List<SqlParameter>();
            MapToObject = new CityMapToObjectView(logger);
            parms.Add(new SqlParameter("@offset", pager.PageSize * pager.PageNbr));
            parms.Add(new SqlParameter("@pageSize", pager.PageSize));

            storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == FINDALL_PAGEDVIEWPROC);
            if (storedProcedure == null)
            {
                SqlCommandType = Constants.DBCommandType.SQL;
                CMDText = BuildCommandText(FINDALLVIEWPAGER_STMT);
                pager.Entities = await base.FindAll(parms);
            }
            else
            {
                SqlCommandType = Constants.DBCommandType.SPROC;
                parms.Add(new SqlParameter("@sortColumn", pager.SortColumn));
                parms.Add(new SqlParameter("@direction", (int)pager.Direction));
                CMDText = storedProcedure;
                pager.Entities = await base.FindAll(parms);
            }

            CMDText = FINDALLCOUNT_STMT;
            pager.RowCount = await base.FindAllCount();
            return pager;
        }
        #endregion

        #region FindByPK
        public override async Task<City> FindByPK(IPrimaryKey pk)
        {
            SqlCommandType = Constants.DBCommandType.SQL;
            CMDText = FINDBYPK_STMT;
            MapToObject = new CityMapToObject(logger);
            return await base.FindByPK(pk);
        }
        #endregion

        #region Add
        public async Task<object> Add(City entity)
        {
            string storedProcedure = String.Empty;
            object result;
            int key = 0;

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
            MapFromObject = new CityMapFromObject(logger);
            result = await base.Add(entity, entity.PK);
            if (result != null)
                key = Convert.ToInt32(result);
            return key;
        }
        #endregion

        #region Update
        public async Task<int> Update(City entity)
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
            MapFromObject = new CityMapFromObject(logger);
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


