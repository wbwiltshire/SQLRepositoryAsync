using System;
using System.Collections.Generic;

namespace SQLRepositoryAsync.Data
{
    public interface IPager<TEntity>
        where TEntity : class
    {
        int PageNbr { get; set; }
        int PageSize { get; set; }
        string SortColumn { set; get; }
        SQLOrderBy Direction { get; set; }
        string FilterColumn { set; get; }
        object FilterValue { set; get; }
        int RowCount { get; set; }
        ICollection<TEntity> Entities { get; set; }
    }
    public class Pager<TEntity> : IPager<TEntity>
        where TEntity : class
    {
        public int PageNbr { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { set; get; }
        public SQLOrderBy Direction { get; set; }
        public string FilterColumn { get; set; }
        public int RowCount { get; set; }
        public object FilterValue { set; get; }
        public ICollection<TEntity> Entities { get; set; }

        public Pager()
        {
            PageNbr = 0;
            PageSize = 20;
            SortColumn = "Id";                  // Default to Id column
            FilterColumn = String.Empty;        // Default for no filter column
            FilterValue = null;                 // Default for no filter value
            Direction = SQLOrderBy.ASC;         // Default to ASC
        }
    }
}