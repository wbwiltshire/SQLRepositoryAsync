/******************************************************************************************************
 *This class was generated on 01/06/2020 10:09:53 using Development Center version 0.9.2. *
 *The class was generated from Database: DCustomer and Table: ProjectContact.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data.Interfaces;

namespace SQLRepositoryAsync.Data.POCO
{

	public class ProjectContact
	{

		public PrimaryKey PK { get; set; }
		[Display(Name = "Project Id")]
		public int ProjectId
		{
			get { return (int)PK.CompositeKey[0]; }
			set { PK.CompositeKey[0] = (int)value; }
		}
		[Display(Name = "Contact Id")]
		public int ContactId
		{
			get { return (int)PK.CompositeKey[1]; }
			set { PK.CompositeKey[1] = (int)value; }
		}
		[Display(Name = "Is Active")]
		public bool Active { get; set; }
		[Display(Name = "Modified Date")]
		public DateTime ModifiedDt { get; set; }
		[Display(Name = "Create Date")]
		public DateTime CreateDt { get; set; }
		public ProjectContact()
		{
			PK = new PrimaryKey() { 
				Key = -1, 
				CompositeKey = new object[] { -1, -1 },
				IsComposite = true, 
				IsIdentity = false
			};
		}
		public override string ToString()
		{
			return $"{ProjectId}|{ContactId}|{Active}|{ModifiedDt}|{CreateDt}|";
		}

		//Relation properties
		public Project Project { get; set; }
		public Contact Contact { get; set; }
	}

	public class ProjectContactMapToObject : MapToObjectBase<ProjectContact>, IMapToObject<ProjectContact>
	{
		public ProjectContactMapToObject(ILogger l) : base(l)
		{
		}

		public override ProjectContact Execute(IDataReader reader)
		{
			ProjectContact projectContact = new ProjectContact();
			int ordinal = 0;
			try
			{
				ordinal = reader.GetOrdinal("ProjectId");
				projectContact.ProjectId = reader.GetInt32(ordinal);
				ordinal = reader.GetOrdinal("ContactId");
				projectContact.ContactId = reader.GetInt32(ordinal);
				ordinal = reader.GetOrdinal("Active");
				projectContact.Active = reader.GetBoolean(ordinal);
				ordinal = reader.GetOrdinal("ModifiedDt");
				projectContact.ModifiedDt = reader.GetDateTime(ordinal);
				ordinal = reader.GetOrdinal("CreateDt");
				projectContact.CreateDt = reader.GetDateTime(ordinal);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
				projectContact = null;
			}
			return projectContact;
		}
	}


	public class ProjectContactMapToObjectView : MapToObjectBase<ProjectContact>, IMapToObject<ProjectContact>
	{
		public ProjectContactMapToObjectView(ILogger l) : base(l)
		{
		}

		public override ProjectContact Execute(IDataReader reader)
		{
			IMapToObject<ProjectContact> map = new ProjectContactMapToObject(logger);
			ProjectContact projectContact = map.Execute(reader);
			try
			{
				projectContact.Project = new Project
				{
					PK = new PrimaryKey { Key = projectContact.ProjectId, IsIdentity = true },
					Name = reader.GetString(reader.GetOrdinal("ProjectName"))
				};
				projectContact.Contact = new Contact
				{
					PK = new PrimaryKey { Key = projectContact.ContactId, IsIdentity = true },
					FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
					LastName = reader.GetString(reader.GetOrdinal("LastName")),
					Address1 = reader.GetString(reader.GetOrdinal("Address1")),
					Address2 = reader.GetString(reader.GetOrdinal("Address2")),
					CellPhone = reader.GetString(reader.GetOrdinal("CellPhone")),
					CityId = reader.GetInt32(reader.GetOrdinal("CityId")),
					EMail = reader.GetString(reader.GetOrdinal("EMail")),
					HomePhone = reader.GetString(reader.GetOrdinal("HomePhone"))
				};
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return projectContact;
		}
	}
	public class ProjectContactMapFromObject : MapFromObjectBase<ProjectContact>, IMapFromObject<ProjectContact>
	{
		public ProjectContactMapFromObject(ILogger l) : base(l)
		{
		}

		public override void Execute(ProjectContact projectContact, SqlCommand cmd)
		{
			SqlParameter parm;

			try
			{
				parm = new SqlParameter("@p1", projectContact.ProjectId);
				cmd.Parameters.Add(parm);
				parm = new SqlParameter("@p2", projectContact.ContactId);
				cmd.Parameters.Add(parm);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
		}
	}
}