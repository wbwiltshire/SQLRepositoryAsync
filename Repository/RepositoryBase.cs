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
 
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        private UnitOfWork unitOfWork;
        private readonly ILogger logger;
        private DBConnection db;

        #region ctor
        //ctor with no unit of work necessary
        protected RepositoryBase(AppSettingsConfiguration s, ILogger l)
        {
            Settings = s;
            logger = l;
            //Setup singleton for DB Connection instance
            db = new DBConnection(Settings.Database.ConnectionString, l);
        }
        //ctor with unit of work
        protected RepositoryBase(ILogger l, UnitOfWork uow)
        {
            logger = l;
            //Setup singleton for DB Connection instance
            unitOfWork = uow;
            db = unitOfWork.DBconnection;
            Settings = unitOfWork.Settings;
        }
        #endregion

        public string OrderBy { get; set; }
        protected string CMDText { get; set; }
        protected AppSettingsConfiguration Settings { get; }
        protected Constants.DBCommandType SqlCommandType { get; set; }
        //protected MapperBase<TEntity> Mapper { get; set; }
        protected MapToObjectBase<TEntity> MapToObject { get; set; }
        protected MapFromObjectBase<TEntity> MapFromObject { get; set; }

        protected async Task<int> FindAllCount()
        {
            object cnt;

            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    //Returns an object, not an int
                    cnt = await cmd.ExecuteScalarAsync();
                    logger.LogInformation("FindAllCount complete.");
                    if (cnt != null)
                        return Convert.ToInt32(cnt);
                    else
                        return 0;
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return 0;
            }
        }

        public virtual async Task<ICollection<TEntity>> FindAll()
        {
            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        ICollection<TEntity> entities = new List<TEntity>();
                        while (await reader.ReadAsync())
                        {
                            entities.Add(MapToObject.Execute(reader));
                        }
                        logger.LogInformation($"FindAll complete for {typeof(TEntity)} entity.");
                        return entities;
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public virtual async Task<ICollection<TEntity>> FindAllPaged(int offset, int pageSize)
        {
            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@p1", offset));
                    cmd.Parameters.Add(new SqlParameter("@p2", pageSize));

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        ICollection<TEntity> entities = new List<TEntity>();
                        while (await reader.ReadAsync())
                        {
                            entities.Add(MapToObject.Execute(reader));
                        }
                        logger.LogInformation($"FindAllPaged complete for {typeof(TEntity)} entity.");
                        return entities;
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public virtual async Task<TEntity> FindByPK(IPrimaryKey pk)
        {
            TEntity entity = null;

            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            try
            {
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                            entity = MapToObject.Execute(reader);
                        else
                            entity = null;
                        logger.LogInformation($"FindByPK complete for {typeof(TEntity)} entity.");
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return null;
            }

            return entity;
        }

        protected async Task<object> Add(TEntity entity, PrimaryKey pk)
        {
            object result = null;
            int rows = 0;

            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();
            
            try
            {
                if (unitOfWork != null) await unitOfWork.Enlist();
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    if (SqlCommandType == Constants.DBCommandType.SPROC)
                        cmd.CommandType = CommandType.StoredProcedure;
                    MapFromObject.Execute(entity, cmd);

                    //If Identity, then it's numeric
                    if (pk.IsIdentity)
                    {
                        //returns PK
                        result = await cmd.ExecuteScalarAsync();
                    }
                    else
                    {
                        //returns rows updated and sets result to key
                        cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                        rows = await cmd.ExecuteNonQueryAsync();
                        result = pk.Key;
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
            }
            logger.LogInformation($"Add complete for {typeof(TEntity)} entity.");
            return result;
        }
 
        protected async Task<int> Update(TEntity entity, IPrimaryKey pk)
        {
            int rows = 0;

            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            try
            {
                if (unitOfWork != null) await unitOfWork.Enlist();
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    if (SqlCommandType == Constants.DBCommandType.SPROC)
                        cmd.CommandType = CommandType.StoredProcedure;
                    MapFromObject.Execute(entity, cmd);
                    cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                    rows = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
            }
            logger.LogInformation($"Update complete for {typeof(TEntity)} entity.");
            return rows;
        }
 
        protected async Task<int> Delete(IPrimaryKey pk)
        {
            int rows = 0;

            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            try
            {
                if (unitOfWork != null) await unitOfWork.Enlist();
                using (SqlCommand cmd = new SqlCommand(CMDText, db.Connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                    rows = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
            }
            logger.LogInformation($"Delete complete for {typeof(TEntity)} entity.");
            return rows;
        }

        #region Ping
        public async Task<bool> Ping()
        {
            if (db.Connection.State != ConnectionState.Open)
                await db.OpenConnection();

            //return true if open and false if closed
            return db.Connection.State == ConnectionState.Open;
        }
        #endregion

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //We'll close here, if no UOW.  Otherwise, close when UOW disposed
            if (unitOfWork == null)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        if (db.Connection != null)
                        {
                            logger.LogInformation("Disposing of SQL Connection from Repository Base");
                            db.Close();
                            db.Connection.Dispose();
                        }
                    }
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
