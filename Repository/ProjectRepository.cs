/******************************************************************************************************
 *This class was generated on 01/02/2020 06:43:02 using Development Center version 0.9.2. *
 *The class was generated from Database: DCustomer and Table: Project.  *
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

	public class ProjectRepository : RepositoryBase<Project>, IRepository<Project>
	{
		private const string FINDALLCOUNT_STMT = "SELECT COUNT(Id) FROM Project WHERE Active=1;";
		private const string FINDALL_STMT = "SELECT Id,Name,Active,ModifiedUtcDt,CreateUtcDt FROM Project WHERE Active=1";
		private const string FINDALLVIEW_STMT = "";
		private const string FINDALLPAGER_STMT = "SELECT Id,Name,Active,ModifiedUtcDt,CreateUtcDt FROM Project WHERE Active=1 @orderBy OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;";
		private const string FINDALLVIEWPAGER_STMT = "";
		private const string FINDBYPK_STMT = "SELECT Id,Name,Active,ModifiedUtcDt,CreateUtcDt FROM Project WHERE Id=@pk AND Active=1;";
		private const string FINDBYPKVIEW_STMT = "";
		private const string ADD_STMT = "INSERT INTO Project (Name, Active, ModifiedUtcDt, CreateUtcDt) VALUES (@p1,  1, GETDATE(), GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);";
		private const string UPDATE_STMT = "UPDATE Project SET Name=@p1, ModifiedUtcDt=GETDATE() WHERE Id=@pk;";
		private const string DELETE_STMT = "UPDATE Project SET Active=0, ModifiedUtcDt=GETDATE() WHERE Id=@pk;";
		private const string ORDERBY_STMT = " ORDER BY ";
		private const string FINDALL_PAGEDPROC = "uspFindAllProjectPaged";
		private const string FINDALL_PAGEDVIEWPROC = "uspFindAllProjectPagedView";
		private const string ADD_PROC = "uspAddProject";
		private const string UPDATE_PROC = "uspUpdateProject";

		private ILogger logger;

		#region ctor
		//Default constructor calls the base ctor
		public ProjectRepository(AppSettingsConfiguration s, ILogger l, DBContext d) :
			base(s, l, d)
		{ Init(l); }
		public ProjectRepository(AppSettingsConfiguration s, ILogger l, UnitOfWork uow, DBContext d) :
			base(s, l, uow, d)
		{ Init(l); }

		private void Init(ILogger l)
		{
			logger = l;
			OrderByColumns = new Dictionary<string, int>() { { "Id", 1 } };
			AddOrderByStatement("Id", SQLOrderBy.ASC);
		}
		#endregion

		#region FindAll
		public override async Task<ICollection<Project>> FindAll()
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = FINDALL_STMT + OrderBy;
			MapToObject = new ProjectMapToObject(logger);
			return await base.FindAll();
		}
		#endregion

		#region FindAll(IPager pager)
		public async Task<IPager<Project>> FindAll(IPager<Project> pager)
		{
			string storedProcedure = String.Empty;
			IList<SqlParameter> parms = new List<SqlParameter>();
			MapToObject = new ProjectMapToObject(logger);
			parms.Add(new SqlParameter("@offset", pager.PageSize * pager.PageNbr));
			parms.Add(new SqlParameter("@pageSize", pager.PageSize));

			storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == FINDALL_PAGEDPROC);
			if (storedProcedure == null)
			{
				AddOrderByStatement(pager.SortColumn, pager.Direction);
				SqlCommandType = Constants.DBCommandType.SQL;
				CMDText = BuildCommandText(FINDALLPAGER_STMT);
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

		#region FindAllView
		public Task<ICollection<Project>> FindAllView()
		{
			throw new NotSupportedException("Project views are not provided.");
		}
		#endregion

		#region FindAllView(IPager pager)
		public Task<IPager<Project>> FindAllView(IPager<Project> pager)
		{
			throw new NotSupportedException("Project views are not provided.");
		}
		#endregion
		
		#region FindAllFiltered(Pager)
		public async Task<IPager<Project>> FindAllFiltered(IPager<Project> pager)
		{
			await Task.Run(() => { throw new NotImplementedException(); });
			return pager;
		}
		#endregion

		#region FindByPK(IPrimaryKey pk)
		public override async Task<Project> FindByPK(IPrimaryKey pk)
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = FINDBYPK_STMT;
			MapToObject = new ProjectMapToObject(logger);
			return await base.FindByPK(pk);
		}
		#endregion

		#region FindViewByPK(IPrimaryKey pk)
		public Task<Project> FindByPKView(IPrimaryKey pk)
		{
			throw new NotSupportedException("Project views are not provided.");
		}
		#endregion

		#region Add(Project)
		public async Task<object> Add(Project entity)
		{
			string storedProcedure = String.Empty;

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
			MapFromObject = new ProjectMapFromObject(logger);
			return await base.Add(entity, entity.PK);
		}
		#endregion

		#region Update(Project)
		public async Task<int> Update(Project entity)
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
			MapFromObject = new ProjectMapFromObject(logger);
			return await base.Update(entity, entity.PK);
		}
		#endregion

		#region Delete(PrimaryKey pk)
		public async Task<int> Delete(PrimaryKey pk)
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = DELETE_STMT;
			return await base.Delete(pk);
		}
		#endregion

	}
}