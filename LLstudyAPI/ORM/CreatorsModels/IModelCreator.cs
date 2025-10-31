using System.Data;

namespace LLstudyWS.ORM
{
    public interface IModelCreator<T>
    {
        T CreateModel(IDataReader dataReader);
        
    }
}
