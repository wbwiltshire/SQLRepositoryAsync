﻿using System;
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
     public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        private UnitOfWork unitOfWork;
        private readonly ILogger logger;
        private DBContext dbc;

        #region ctor
        //ctor with no unit of work necessary
        protected RepositoryBase(AppSettingsConfiguration s, ILogger l, DBContext d)
        {
            Settings = s;
            logger = l;
            dbc = d;
        }
        //ctor with unit of work
        protected RepositoryBase(AppSettingsConfiguration s, ILogger l, UnitOfWork uow, DBContext d)
        {
            Settings = s;
            logger = l;
            unitOfWork = uow;
            dbc = d;
        }
        #endregion

        protected string OrderBy { get; set; }
        protected string CMDText { get; set; }
        protected AppSettingsConfiguration Settings { get; private set; }
        protected Constants.DBCommandType SqlCommandType { get; set; }
        protected MapToObjectBase<TEntity> MapToObject { get; set; }
        protected MapFromObjectBase<TEntity> MapFromObject { get; set; }
        protected Dictionary<string, int> OrderByColumns { get; set; }
        protected void AddOrderByStatement(string columnName, SQLOrderBy direction)
        {
            string safeColumnName = "Id";                        // Set as default
            if (OrderByColumns is not null && OrderByColumns.ContainsKey(columnName))
                safeColumnName = columnName;
                
            OrderBy = $"ORDER BY {safeColumnName} " + (direction == SQLOrderBy.ASC ? "ASC" : "DESC");
        }
        protected IEnumerable<SqlParameter> AddOrderByParmeters(string columnName, SQLOrderBy direction)
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            int columnId = 1;                                   // Set as default

            if (OrderByColumns is not null)
                columnId = OrderByColumns[columnName];
            parms.Add(new SqlParameter("@sortColumn",columnId));
            parms.Add(new SqlParameter("@direction", (int)direction));

            return parms;
        }
        protected string ReplaceFilterColumn(string columnName, string cmdText)
        {
            if (OrderByColumns is not null && OrderByColumns.ContainsKey(columnName))
                return cmdText.Replace("@filterColumn", columnName);
            else
                return cmdText.Replace("@filterColumn = @filterValue AND ", "");
        }
        protected string BuildCommandText(string ct)
        {
            if (ct.Contains("@orderBy"))
                return ct.Replace("@orderBy", OrderBy);
            else
                return ct;
        }

        #region FindAllCount
        protected async Task<int> FindAllCount()
        {
            object cnt;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
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
        #endregion

        #region FindAll
        public virtual async Task<ICollection<TEntity>> FindAll()
        {
            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
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
        #endregion

        #region FindAll(parms)
        public virtual async Task<ICollection<TEntity>> FindAll(IList<SqlParameter> parms)
        {
            ICollection<TEntity> entities;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    if (SqlCommandType == Constants.DBCommandType.SPROC)
                        cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parm in parms)
                        cmd.Parameters.Add(parm);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        entities = new List<TEntity>();
                        while (await reader.ReadAsync())
                        {
                            entities.Add(MapToObject.Execute(reader));
                        }
                        logger.LogInformation($"FindAll(SqlParameter) complete for {typeof(TEntity)} entity.");
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
        #endregion

        #region FindByPK
        public virtual async Task<TEntity> FindByPK(IPrimaryKey pk)
        {
            int idx = 1;
            TEntity entity = null;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    if (pk.IsComposite) { 
                        foreach (int k in ((PrimaryKey)pk).CompositeKey) {
                            cmd.Parameters.Add(new SqlParameter("@pk" + idx.ToString(), k));
                            idx++;
                        }
                    }
                    else
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
        #endregion

        #region Add
        protected async Task<object> Add(TEntity entity, PrimaryKey pk)
        {

            object result = null;
            int rows = 0;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                if (unitOfWork != null) await unitOfWork.Enlist();
                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    if (SqlCommandType == Constants.DBCommandType.SPROC)
                        cmd.CommandType = CommandType.StoredProcedure;
                    MapFromObject.Execute(entity, cmd);

                    //If Composite, then returns an array of objects
                    if (pk.IsComposite)
                    {
                        //returns CompositeKey
                        rows = await cmd.ExecuteNonQueryAsync();
                        result = rows;
                    }
                    //If Identity, then it's numeric
                    else if (pk.IsIdentity)
                    {
                        //returns PK
                        result = await cmd.ExecuteScalarAsync();
                    }
                    //Else it's a natural key
                    else
                    {
                        //returns rows updated and sets result to key
                        cmd.Parameters.Add(new SqlParameter("@pk", pk.Key));
                        rows = await cmd.ExecuteNonQueryAsync();
                        result = rows;
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
        #endregion

        #region Update
        protected async Task<int> Update(TEntity entity, IPrimaryKey pk)
        {
            int rows = 0;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                if (unitOfWork != null) await unitOfWork.Enlist();
                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
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
        #endregion

        #region Delete
        protected async Task<int> Delete(IPrimaryKey pk)
        {
            int idx = 1;
            int rows = 0;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                if (unitOfWork != null) await unitOfWork.Enlist();
                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    if (pk.IsComposite)
                    {
                        foreach (int k in ((PrimaryKey)pk).CompositeKey)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pk" + idx.ToString(), k));
                            idx++;
                        }
                    }
                    else
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
        #endregion

        #region ExecNonQuery
        protected async Task<int> ExecNonQuery(IList<SqlParameter> p)
        {
            int rows = 0;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    if (SqlCommandType == Constants.DBCommandType.SPROC)
                        cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter s in p)
                        cmd.Parameters.Add(s);

                    //Returns an object, not an int
                    rows = await cmd.ExecuteNonQueryAsync();
                    logger.LogInformation("ExecNonQuery complete.");
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                rows = 0;
            }

            return rows;
        }
        #endregion

        #region ExecStoredProc
        protected async Task<int> ExecStoredProc(IList<SqlParameter> p)
        {
            int cnt = 0;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();


                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter s in p)
                        cmd.Parameters.Add(s);

                    cnt = await cmd.ExecuteNonQueryAsync();
                    logger.LogInformation("ExecStoredProc complete.");
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return 0;
            }
            return cnt;
        }
        #endregion

        #region ExecJSONQuery
        protected async Task<string> ExecJSONQuery(IList<SqlParameter> parms)
        {
            string result = String.Empty;

            try
            {
                if (dbc.Connection.State != ConnectionState.Open)
                    await dbc.Open();

                using (SqlCommand cmd = new SqlCommand(CMDText, dbc.Connection))
                {
                    if (SqlCommandType == Constants.DBCommandType.SPROC)
                        cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parm in parms)
                        cmd.Parameters.Add(parm);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        //Returns a string
                        while (await reader.ReadAsync())
                            result += reader.GetString(0);
                    }
                    logger.LogInformation("ExecJSONQuery complete.");
                    return result;
                }
            }
            catch (SqlException ex)
            {
                logger.LogError(ex.Message);
                return "{}";
            }
        }
        #endregion
    }
}
