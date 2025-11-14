
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using System.Reflection;

namespace LLstudyWS.ORM
{
    public class Repository<T>: IRepository<T> where T: new()
    {
        protected DbHelperOledb helperOledb;
        protected ModelCreators modelCreators;
        protected ModelCreatorReflection moderlRefCreator;

        public Repository(DbHelperOledb helperOledb,ModelCreators modelCreator, ModelCreatorReflection modelCreatorReflection)
        {
            this.modelCreators = modelCreator;
            this.moderlRefCreator = modelCreatorReflection;
            this.helperOledb = helperOledb;
        }

        public virtual bool Create(T model)
        {
            
            Type OBJtype = model.GetType();
            List<string> exludePROP = new List<string> { "IsValid", "HasErrors" };
            PropertyInfo[] propertys = OBJtype.GetProperties().Where(p => !exludePROP.Contains(p.Name)).ToArray();
            string columns = string.Join(", ", propertys.Select(p => p.Name));
            string placeholders = string.Join(", ", propertys.Select(p => ("@" + p.Name)));
            string sql = $@"INSERT INTO {OBJtype.Name}s ({columns}) VALUES({placeholders})";
            Console.WriteLine(sql);


            Type PropretyType;
            string? value;
            foreach (PropertyInfo pro in propertys)
            {
                PropretyType = pro.PropertyType;
                value = Convert.ToString(Convert.ChangeType(pro.GetValue(model, null), PropretyType));
                this.helperOledb.AddParameter(("@" + pro.Name), (string)value);
            }
          
            return this.helperOledb.Insert(sql) > 0;



            //string sql = @$"INSERT INTO {model.GetType().Name}s (";

            //foreach(PropertyInfo pro in propertys)
            //{
            //    sql += pro.Name + ",";
            //}
            //sql += ") VALUES (";
            //foreach (PropertyInfo pro in propertys)
            //{
            //    sql += "@" + pro.Name + ",";
            //}
            //sql += ")";



            //string sql2 = $@"INSERT INTO Books (
            //                author_name,
            //                book_name,
            //                book_price,
            //                In_stock,
            //                subjectID,
            //                pdf_url_book
            //                )
            //            VALUES
            //                (@AuthorName, @BookName, @BookPrice, @InStock, @SubjectID, @PdfUrlBook) ";

            //this.helperOledb.AddParameter("@AuthorName", model.Author_name);
            //this.helperOledb.AddParameter("@BookName", model.Book_name);
            //this.helperOledb.AddParameter("@BookPrice", model.Book_price.ToString());
            //this.helperOledb.AddParameter("@InStock", model.In_stock.ToString());
            //this.helperOledb.AddParameter("@SubjectID", model.SubjectID);
            //this.helperOledb.AddParameter("@PdfUrlBook", model.Pdf_url_book);



        }

        public virtual bool Delete(string id)
        {
            Type typeObj = typeof(T);
            PropertyInfo prop = typeObj.GetProperty(@$"{typeObj.Name}ID");
            string sql = @$"DELETE * FROM {typeObj.Name}s WHERE {prop.Name} = @ID";
            this.helperOledb.AddParameter("@ID", id);
            return this.helperOledb.Delete(sql) > 0;
        }

        public virtual List<T> GetAll()
        {
            List<T> records = new List<T>();

            string Class_name = typeof(T).Name;
            string sql = $@"SELECT * FROM {Class_name}s";
            Console.WriteLine(sql);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {

                    records.Add(this.moderlRefCreator.CreateModel<T>(reader));
                }
            }
            return records;
        }

        public virtual T GetByID(string ID)
        {
            Type objType = typeof(T);
            T obj;
            PropertyInfo propID = objType.GetProperty($@"{objType.Name}ID");

            string sql = $@"SELECT * FROM {objType.Name}s WHERE {propID.Name} = @ID";
            this.helperOledb.AddParameter("@ID",ID);
            using(IDataReader reader  = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    obj = this.moderlRefCreator.CreateModel<T>(reader);
                    return obj;
                }
            }
            return new T();
        }

        public bool Update(T model)
        {
            string sql;
            Type typeProp = model.GetType();
            List<string> exludedProps = new List<string>() { "IsValid", "HasErrors"};
            PropertyInfo[] props = typeProp.GetProperties().Where(p => (!exludedProps.Contains(p.Name) && !p.Name.Equals($@"{p.Name}ID"))).ToArray();
            string sets = string.Join(", ", props.Select(p => @$"@{p.Name} = {p.GetValue(model,null)}"));

            PropertyInfo propID = typeProp.GetProperty(@$"{typeProp.Name}ID");

            sql = $@"UPDATE {typeProp.Name}s SET {sets} WHERE {propID.Name} = @{propID.Name}";

            foreach(PropertyInfo pro in props)
            {
                this.helperOledb.AddParameter(@$"@{pro.Name}", pro.GetValue(model, null).ToString());
            }

            this.helperOledb.AddParameter($@"@{propID.Name}", propID.GetValue(model, null).ToString());

            return this.helperOledb.Update(sql) > 0;



        }
    }
}
