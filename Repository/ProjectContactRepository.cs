/******************************************************************************************************
 *This class was generated on 01/06/2020 10:33:30 using Development Center version 0.9.2. *
 *The class was generated from Database: DCustomer and Table: ProjectContact.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.Interfaces;
using SQLRepositoryAsync.Data.POCO;

namespace SQLRepositoryAsync.Data.Repository
{

	public class ProjectContactRepository : RepositoryBase<ProjectContact>, IRepository<ProjectContact>
	{
		private const string FINDALLCOUNT_STMT = "SELECT COUNT(ProjectId) FROM ProjectContact WHERE Active=1";
		private const string FINDALL_STMT = "SELECT ProjectId,ContactId,Active,ModifiedDt,CreateDt FROM ProjectContact WHERE Active=1";
		private const string FINDALLVIEW_STMT = "";
		private const string FINDALLPAGER_STMT = "";
		private const string FINDALLVIEWPAGER_STMT = "";
		private const string FINDBYPK_STMT = "SELECT ProjectId, ContactId, Active, ModifiedDt, CreateDt FROM ProjectContact WHERE ProjectId=@pk1 AND ContactId=@pk2 AND Active=1";
		private const string FINDBYPKVIEW_STMT = "";
		private const string ADD_STMT = "INSERT INTO ProjectContact (ProjectId, ContactId, Active, ModifiedDt, CreateDt) VALUES (@p1, @p2, 1, GETDATE(), GETDATE())";
		private const string UPDATE_STMT = "";
		private const string DELETE_STMT = "UPDATE ProjectContact SET Active=0, ModifiedDt=GETDATE() WHERE ProjectId=@pk1 AND ContactId=@pk2";
		private const string ORDERBY_STMT = " ORDER BY ";
		private const string FINDALL_PAGEDPROC = "uspFindAllProjectContactPaged";
		private const string FINDALL_PAGEDVIEWPROC = "uspFindAllProjectContactPagedView";
		private const string ADD_PROC = "uspAddProjectContact";
		private const string UPDATE_PROC = "uspUpdateProjectContact";

		private ILogger logger;

		#region ctor
		//Default constructor calls the base ctor
		public ProjectContactRepository(AppSettingsConfiguration s, ILogger l, DBConnection d) :
			base(s, l, d)
		{ Init(l); }
		public ProjectContactRepository(AppSettingsConfiguration s, ILogger l, UnitOfWork uow, DBConnection d) :
			base(s, l, uow, d)
		{ Init(l); }

		private void Init(ILogger l)
		{
			logger = l;
			OrderBy = "ProjectId, ContactId";
		}
		#endregion

		#region FindAll
		public override async Task<ICollection<ProjectContact>> FindAll()
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = FINDALL_STMT;
			CMDText += ORDERBY_STMT + OrderBy;
			MapToObject = new ProjectContactMapToObject(logger);
			return await base.FindAll();
		}
		#endregion

		#region FindAll(IPager pager)
		public async Task<IPager<ProjectContact>> FindAll(IPager<ProjectContact> pager)
		{
			string storedProcedure = String.Empty;

			MapToObject = new ProjectContactMapToObject(logger);

			storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == FINDALL_PAGEDPROC);
			if (storedProcedure == null)
			{
				SqlCommandType = Constants.DBCommandType.SQL;
				CMDText = String.Format(FINDALLPAGER_STMT, pager.PageSize * pager.PageNbr, pager.PageSize);
				pager.Entities = await base.FindAll();
			}
			else
			{
				SqlCommandType = Constants.DBCommandType.SPROC;
				CMDText = storedProcedure;
				pager.Entities = await base.FindAllPaged(pager.PageSize * pager.PageNbr, pager.PageSize);
			}
			CMDText = FINDALLCOUNT_STMT;
			pager.RowCount = await base.FindAllCount();
			return pager;
		}
		#endregion

		#region FindAllView
		public async Task<ICollection<ProjectContact>> FindAllView()
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = String.Format(FINDALLVIEW_STMT);
			CMDText += ORDERBY_STMT + OrderBy;
			MapToObject = new ProjectContactMapToObjectView(logger);
			return await base.FindAll();
		}
		#endregion

		#region FindAllView(IPager pager)
		public async Task<IPager<ProjectContact>> FindAllView(IPager<ProjectContact> pager)
		{
			string storedProcedure = String.Empty;

			MapToObject = new ProjectContactMapToObjectView(logger);

			storedProcedure = Settings.Database.StoredProcedures.FirstOrDefault(p => p == FINDALL_PAGEDVIEWPROC);
			if (storedProcedure == null)
			{
				SqlCommandType = Constants.DBCommandType.SQL;
				CMDText = String.Format(FINDALLVIEWPAGER_STMT, pager.PageSize * pager.PageNbr, pager.PageSize);
				pager.Entities = await base.FindAll();
			}
			else
			{
				SqlCommandType = Constants.DBCommandType.SPROC;
				CMDText = storedProcedure;
				pager.Entities = await base.FindAllPaged(pager.PageSize * pager.PageNbr, pager.PageSize);
			}
			CMDText = FINDALLCOUNT_STMT;
			pager.RowCount = await base.FindAllCount();
			return pager;
		}
		#endregion

		#region FindByPK(IPrimaryKey pk)
		public override async Task<ProjectContact> FindByPK(IPrimaryKey pk)
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = FINDBYPK_STMT;
			MapToObject = new ProjectContactMapToObject(logger);
			return await base.FindByPK(pk);
		}
		#endregion

		#region FindViewByPK(IPrimaryKey pk)
		public async Task<ProjectContact> FindByPKView(IPrimaryKey pk)
		{
			SqlCommandType = Constants.DBCommandType.SQL;
			CMDText = FINDBYPKVIEW_STMT;
			MapToObject = new ProjectContactMapToObjectView(logger);
			return await base.FindByPK(pk);
		}
		#endregion

		#region Add(ProjectContact)
		public async Task<object> Add(ProjectContact entity)
		{
			string storedProcedure = String.Empty;
			int result;

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
			MapFromObject = new ProjectContactMapFromObject(logger);
			result = (int)await base.Add(entity, entity.PK);

			//Number of rows that were added
			return result;
		}
		#endregion

		#region Update(ProjectContact)
		public async Task<int> Update(ProjectContact entity)
		{
			//await Task.Delay(1);
			await Task.Run(() => logger.LogError("Update(entity) Project Contact not implemented"));
			throw new NotImplementedException("Update(entity) Project Contact not implemented");
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