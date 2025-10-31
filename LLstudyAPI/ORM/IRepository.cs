namespace LLstudyWS.ORM
{
    public interface IRepository<T>
    {
        bool Create();
        bool Update();
        bool Delete();
        List<T> GetAll();

        T GetByID(string ID);
    }
}
