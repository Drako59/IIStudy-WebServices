namespace LLstudyWS.ORM
{
    public interface IRepository<T>
    {
        bool Create(T model, List<string>? exludes = null);
        bool Update(T model, List<string>? exludes = null);
        bool Delete(string id);
        List<T> GetAll(List<string>? exludes = null);

        T GetByID(string ID, List<string>? exludes = null);
    }
}
