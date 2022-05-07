using System;
using System.Collections.Generic;

namespace SQLRepositoryAsync.Data
{
    public interface IPager<TEntity>
        where TEntity : class
    {
        int PageNbr { get; set; }
        int PageSize { get; set; }
        int SortColumn { set; get; }
        SQLOrderBy Direction { get; set; } 
        int RowCount { get; set; }
        ICollection<TEntity> Entities { get; set; }
    }
    public class Pager<TEntity> : IPager<TEntity>
        where TEntity : class
    {
        public int PageNbr { get; set; }
        public int PageSize { get; set; }
        public int SortColumn { set; get; }
        public SQLOrderBy Direction { get; set; }
        public int RowCount { get; set; }
        public ICollection<TEntity> Entities { get; set; }

        public Pager()
        {
            PageNbr = 1;
            PageSize = 20;
            SortColumn = 1;                   // Default to Id column
            Direction = SQLOrderBy.ASC;       // Default to ASC
        }
    }
}