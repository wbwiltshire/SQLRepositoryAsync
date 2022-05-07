using System;
using System.Collections.Generic;
using System.Text;

namespace SQLRepositoryAsync.Data
{
    public enum SQLOrderBy { ASC = 1, DESC = 2 };

    public class Constants
    {
        public enum DBCommandType { SQL = 1, VIEW = 2, SPROC = 3 };
    }
}
