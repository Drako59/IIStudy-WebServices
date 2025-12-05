using System.Data;

namespace LLstudyWS.ORM.CreatorsModels
{
    public interface IRefModelCreator
    {
        T CreateModel<T>(IDataReader dataReader,List<string>? exludes = null, List<string>? only = null) where T : new();

    }
}
