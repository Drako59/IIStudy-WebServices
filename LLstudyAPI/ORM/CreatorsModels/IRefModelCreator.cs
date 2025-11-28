using System.Data;

namespace LLstudyWS.ORM.CreatorsModels
{
    public interface IRefModelCreator
    {
        T CreateModel<T>(IDataReader dataReader,List<string>? exludes = null) where T : new();

    }
}
