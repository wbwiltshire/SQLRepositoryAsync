/******************************************************************************************************
 *This class was generated on 04/30/2014 09:00:34 using Repository Builder version 0.9. *
 *The class was generated from Database: Customer and Table: Project.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data;
using SQLRepositoryAsync.Data.Interfaces;

namespace SQLRepositoryAsync.Data.POCO
{

    public class Project
    {

        public PrimaryKey PK { get; set; }
        public int Id
        {
            get { return (int)PK.Key; }
            set { PK.Key = (int)value; }
        }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime ModifiedDt { get; set; }
        public DateTime CreateDt { get; set; }
        public Project()
        {
            PK = new PrimaryKey() { Key = -1, IsIdentity = true };
        }
        public override string ToString()
        {
            return $"{Id}|{Name}|{Active}|{ModifiedDt}|{CreateDt}|";
        }

    }

    public class ProjectMapToObject : MapToObjectBase<Project>, IMapToObject<Project>
    {
        public ProjectMapToObject(ILogger l) : base(l)
        {
        }

        public override Project Execute(IDataReader reader)
        {
            Project city = new Project();
            int ordinal = 0;

            try
            {
                ordinal = reader.GetOrdinal("Id");
                city.Id = reader.GetInt32(ordinal);
                ordinal = reader.GetOrdinal("Name");
                city.Name = reader.GetString(ordinal);
                ordinal = reader.GetOrdinal("Active");
                city.Active = reader.GetBoolean(ordinal);
                ordinal = reader.GetOrdinal("ModifiedDt");
                city.ModifiedDt = reader.GetDateTime(ordinal);
                ordinal = reader.GetOrdinal("CreateDt");
                city.CreateDt = reader.GetDateTime(ordinal);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return city;
        }
    }

    public class ProjectMapToObjectView : MapToObjectBase<Project>, IMapToObject<Project>
    {
        public ProjectMapToObjectView(ILogger l) : base(l)
        {

        }

        public override Project Execute(IDataReader reader)
        {
            IMapToObject<Project> map = new ProjectMapToObject(logger);
            Project project = map.Execute(reader);

            try
            {
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return project;
        }
    }

    public class ProjectMapFromObject : MapFromObjectBase<Project>, IMapFromObject<Project>
    {
        public ProjectMapFromObject(ILogger l) : base(l)
        {
        }

        public override void Execute(Project city, SqlCommand cmd)
        {
            SqlParameter parm;

            try
            {
                parm = new SqlParameter("@p1", city.Name);
                cmd.Parameters.Add(parm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}