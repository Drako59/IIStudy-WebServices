using System.Data;
using System.Reflection;

namespace LLstudyWS.ORM.CreatorsModels
{
    public class ModelCreatorReflection : IRefModelCreator
    {
        public T CreateModel<T>(IDataReader dataReader, List<string>? exludes = null, List<string>? only = null) where T  : new()
        {

            Type classType = typeof(T);

            List<string> exludedProp = new List<string>() { "IsValid", "HasErrors" };
            if (exludes != null)
                exludedProp.AddRange(exludes);
            PropertyInfo[] props;
            if (only == null)
                props = classType.GetProperties().Where(p => !exludedProp.Contains(p.Name)).ToArray();
            else
                props = classType.GetProperties().Where(p => only.Contains(p.Name)).ToArray();

            T instance = new T();

            Type ProType;

            foreach(PropertyInfo pro in props)
            {
                ProType = pro.PropertyType;
                pro.SetValue(instance, Convert.ChangeType(dataReader[pro.Name], ProType));
            }

            return instance;
        }

    
    }
}
