namespace LLstudyWS.ORM
{
    public interface IRepository<T>
    {
        bool Create(T model);
        bool Update(T model);
        bool Delete(string id);
        List<T> GetAll();

        T GetByID(string ID);
    }
}
