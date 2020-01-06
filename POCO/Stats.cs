/******************************************************************************************************
 *This class was generated on 04/30/2014 09:00:34 using Repository Builder version 0.9. *
 *The class was generated from Database: Customer and Table: City.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.Interfaces;

namespace SQLRepositoryAsync.Data.POCO
{

    public class Stats
    {
        public string Result { get; set; }

        public Stats()
        {
            Result = String.Empty;
        }
    }

    public class StatsView
    {
        public int TotalContacts { get; set; }
        public int TotalProjects { get; set; }
        public int TotalCities { get; set; }
        public int TotalStates { get; set; }

    }
}