using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data.Interfaces;

namespace SQLRepositoryAsync.Data
{
    public class PrimaryKey : IPrimaryKey
    {
        public object Key { get; set; }
        public bool IsIdentity { get; set; }
    }

    public abstract class MapToObjectBase<TEntity> : IMapToObject<TEntity>
        where TEntity : class
    {
        protected readonly ILogger logger;

        public abstract TEntity Execute(IDataReader reader);
    }
    public abstract class MapFromObjectBase<TEntity> : IMapFromObject<TEntity>
        where TEntity : class
    {
        protected readonly ILogger logger;
        //protected static readonly ILog log = LogManager.GetLogger(typeof(MapToObjectBase<TEntity>));

        public abstract void Execute(TEntity entity, SqlCommand cmd);
    }

}