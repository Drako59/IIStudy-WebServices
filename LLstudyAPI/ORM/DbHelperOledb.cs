using System.Data;
using System.Data.OleDb;

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
            this.oleDbConnection = new OleDbConnection();
            //this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= {Directory.GetCurrentDirectory()} \App_Data\School project .accdb"";Persist Security Info=True";
            this.oleDbConnection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\User\source\repos\IIStudy_Project\LLstudyAPI\App_Data\School project .accdb"";Persist Security Info=True";
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
            return this.dbCommand.ExecuteNonQuery();
        }   

        public int Insert(string sql)
        {
            Console.WriteLine(  "Insert");
            this.dbCommand.CommandText = sql;
            
            return this.dbCommand.ExecuteNonQuery();
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
            return this.dbCommand.ExecuteReader();
        }

        public int Update(string sql)
        {
            this.dbCommand.CommandText = sql;
            return this.dbCommand.ExecuteNonQuery();
        }
    }
}
