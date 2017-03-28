/******************************************************************************************************
 *This class was generated on 04/20/2014 09:21:22 using Repository Builder version 0.9. *
 *The class was generated from Database: BACS and Table: State.  *
******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Logging;
using SQLRepositoryAsync.Data.Interfaces;

namespace SQLRepositoryAsync.Data.POCO
{
    public class State
    {
        public PrimaryKey PK { get; set; }
        public string Id
        {
            get { return (string)PK.Key; }
            set { PK.Key = (string)value; }
        }
        public string Name { get; set; }
        //Properties managed by the architecture
        public bool Active { get; set; }
        public DateTime ModifiedDt { get; set; }
        public DateTime CreateDt { get; set; }
        public State()
        {
            PK = new PrimaryKey() { Key = String.Empty, IsIdentity = false };
        }
        public string ToPrint()
        {
            return String.Format("{0}|{1}|{2}|{3}|{4}", Id, Name, Active, ModifiedDt, CreateDt);
        }

    }


    public class StateMapToObject : MapToObjectBase<State>, IMapToObject<State>
    {
        public override State Execute(IDataReader reader)
        {
            State state = new State();
            try
            {
                state.Id = reader.GetString(0);
                state.Name = reader.GetString(1);
                state.Active = reader.GetBoolean(2);
                state.ModifiedDt = reader.GetDateTime(3);
                state.CreateDt = reader.GetDateTime(4);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return state;
        }
    }
    public class StateMapToObjectView : MapToObjectBase<State>, IMapToObject<State>
    {

        public override State Execute(IDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
    public class StateMapFromObject : MapFromObjectBase<State>, IMapFromObject<State>
    {
        public override void Execute(State state, SqlCommand cmd)
        {
            SqlParameter parm;

            try
            {
                parm = new SqlParameter("@p1", state.Name);
                cmd.Parameters.Add(parm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}