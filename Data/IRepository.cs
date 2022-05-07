﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SQLRepositoryAsync.Data.Interfaces
{
    public enum Action { Add, Update, Delete };

    public interface IPrimaryKey
    {
        object Key { get; set; }
        bool IsIdentity { get; set; }
        bool IsComposite { get; set; }
    }

    public interface IDBContext : IDisposable
    {
        Task<bool> Open();
        void Close();
    }

    public interface IUOW
    {
        Task<bool> Save();
        Task<bool> Rollback();
    }

    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<ICollection<TEntity>> FindAll();
        Task<IPager<TEntity>> FindAll(IPager<TEntity> pager);
        Task<TEntity> FindByPK(IPrimaryKey pk);
        Task<object> Add(TEntity entity);
        Task<int> Update(TEntity entity);
        Task<int> Delete(PrimaryKey pk);
    }

    public interface IMapToObject<TEntity>
        where TEntity : class
    {
        TEntity Execute(IDataReader reader);
    }
    public interface IMapFromObject<TEntity>
        where TEntity : class
    {
        void Execute(TEntity entity, SqlCommand cmd);
    }

}
