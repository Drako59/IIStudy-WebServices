using System.Data;

namespace LLstudyWS
{
    public interface IDbHelper
    {
        void OpenConnection();
        void CloseConnection();
        //records is an object of IDataReader
        IDataReader Select(string sql);
        int Update(string sql);
        int Delete(string sql);
        int Insert(string sql);

        void OpenTransaction();
        void Commit();
        void RollBack();
    }
}
