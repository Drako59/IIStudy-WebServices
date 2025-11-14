using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace LLstudyWS
{
    public class DbHelperOledb : IDbHelper
    {
        //ADO.NET- Access Data Object
        OleDbConnection oleDbConnection;
        OleDbCommand dbCommand;//Resposible for send the command to DB and returns asnwer from the DB
        OleDbTransaction dbTransaction;
        public DbHelperOledb()
        {
            string path = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= ""{Directory.GetCurrentDirectory()}\App_Data\School project .accdb"";Persist Security Info=True";
            this.oleDbConnection = new OleDbConnection();
            //this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= ""{Directory.GetCurrentDirectory()}\App_Data\School project .accdb"";Persist Security Info=True";
            //this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= ""C:\Users\ayele\source\repos\Drako59\IIStudy-WebServices\LLstudyAPI\App_Data\School project .accdb"";Persist Security Info=True";
            this.oleDbConnection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\User\source\repos\Drako59\IIStudy-WebServices\LLstudyAPI\App_Data\School project .accdb"";Persist Security Info=True";
            this.dbCommand = new OleDbCommand();
            this.dbCommand.Connection = this.oleDbConnection;
        }


        public void CloseConnection()
        {
            this.oleDbConnection.Close();
        }

        public void Commit()
        {
            this.dbTransaction.Commit();
        }

        public int Delete(string sql)
           
        {
            this.dbCommand.CommandText = sql;
            
            int record =  this.dbCommand.ExecuteNonQuery();
            this.dbCommand.Parameters.Clear();
            return record;
        }   

        public int Insert(string sql)
           
        {
            
            Console.WriteLine(  "Insert");
            this.dbCommand.CommandText = sql;
            
            int record =  this.dbCommand.ExecuteNonQuery();
            this.dbCommand.Parameters.Clear();
            return record;
        }

        public void OpenConnection()
        {
            this.oleDbConnection.Open();
        }

        public void OpenTransaction()
        {
            this.dbTransaction = this.oleDbConnection.BeginTransaction();
        }

        public void RollBack()
        {
            this.dbTransaction.Rollback();
        }

        

        public IDataReader Select(string sql)
        {
            this.dbCommand.CommandText = sql;
            IDataReader dataReader = this.dbCommand.ExecuteReader();
            this.dbCommand.Parameters.Clear();
            return dataReader;
        }

        public int Update(string sql)
        {
            this.dbCommand.CommandText = sql;
            int record  =  this.dbCommand.ExecuteNonQuery();
            this.dbCommand.Parameters.Clear();
            return record;
        }

        //public void AddParameter<T>(string name,T  value)
        public void AddParameter(string name,object  value)
        {
            this.dbCommand.Parameters.Add(new OleDbParameter(name, value));
        }
    }
}
