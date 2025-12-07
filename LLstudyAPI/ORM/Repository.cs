
using LLstudyWS.ORM.CreatorsModels;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

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

        public virtual bool Create(T model, List<string>? exludes = null)
        {
            
            Type OBJtype = model.GetType();
            List<string> exludePROP = new List<string> { "IsValid", "HasErrors" };
            if (exludes != null)
                exludePROP.AddRange(exludes);
            PropertyInfo[] propertys = OBJtype.GetProperties().Where(p => (!exludePROP.Contains(p.Name) && (!p.Name.Equals(OBJtype.Name + "ID")))).ToArray();
            //foreach (PropertyInfo pro in propertys)
            //{
            //    Console.WriteLine(@$"Property Name: {pro.Name}");
            //}
            string columns = string.Join(", ", propertys.Select(p => $@"[{p.Name}]"));
            string placeholders = string.Join(", ", propertys.Select(p => ("@" + p.Name)));
            string sql = $@"INSERT INTO {OBJtype.Name}s ({columns}) VALUES({placeholders})";
            Console.WriteLine(sql);
            string salt = GetSalt(GetRandomNumber());

            Type PropretyType;
            string? value;
            foreach (PropertyInfo pro in propertys)
            {
               

                PropretyType = pro.PropertyType;
                //value = Convert.ToString(Convert.ChangeType(pro.GetValue(model, null), PropretyType));
                if (pro.Name.Equals($@"{OBJtype.Name}Salt")) {
                    this.helperOledb.AddParameter(("@" + pro.Name.ToString()),  salt);
                }
                else if(pro.Name.Equals("Password"))
                    this.helperOledb.AddParameter(("@" + pro.Name.ToString()), GetHash((string)pro.GetValue(model, null), salt));
                else
                    this.helperOledb.AddParameter(("@" + pro.Name.ToString()), pro.GetValue(model, null) ?? "");



                Console.WriteLine(@$"{"@" + pro.Name}:  {pro.GetValue(model, null)?.ToString() ?? ""} ");
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
        
        protected int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(8, 16);
        }
        
        protected string GetHash(string password, string salt)
        {
            string combine = password + salt;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(combine);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private string GetSalt(int length)
        {
            byte[] bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            string s = Convert.ToBase64String(bytes);
            return s;
        }
        public virtual bool Delete(string id)
        {
            Type typeObj = typeof(T);
            PropertyInfo prop = typeObj.GetProperty(@$"{typeObj.Name}ID");
            string sql = @$"DELETE * FROM {typeObj.Name}s WHERE {prop.Name} = @ID";
            this.helperOledb.AddParameter("@ID", id);
            return this.helperOledb.Delete(sql) > 0;
        }

        public virtual List<T> GetAll(List<string>? exludes = null)
        {
            List<T> records = new List<T>();

            string Class_name = typeof(T).Name;
            string sql = $@"SELECT * FROM {Class_name}s";
            Console.WriteLine(sql);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {

                    records.Add(this.moderlRefCreator.CreateModel<T>(reader, exludes: exludes));
                }
            }
            return records;
        }

        public virtual T GetByID(string ID, List<string>? exludes = null)
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
                    obj = this.moderlRefCreator.CreateModel<T>(reader, exludes: exludes);
                    return obj;
                }
            }
            return new T();
        }

        public virtual bool Update(T model, List<string>? exludes = null)
        {
            string sql;
            Type typeProp = model.GetType();
            List<string> exludedProps = new List<string>() { "IsValid", "HasErrors"};
            if (exludes != null)
                exludedProps.AddRange(exludes);
            PropertyInfo[] props = typeProp.GetProperties().Where(p => (!exludedProps.Contains(p.Name) && !p.Name.Equals($@"{p.Name}ID"))).ToArray();
            string sets = string.Join(", ", props.Select(p => @$"{p.Name} = @{p.Name}"));

            PropertyInfo propID = typeProp.GetProperty(@$"{typeProp.Name}ID");

            sql = $@"UPDATE {typeProp.Name}s SET {sets} WHERE {propID.Name} = @{propID.Name}";

            foreach(PropertyInfo pro in props)
            {
                this.helperOledb.AddParameter(@$"@{pro.Name}", pro.GetValue(model, null) ?? "");
            }

            this.helperOledb.AddParameter($@"@{propID.Name}", propID.GetValue(model, null));

            return this.helperOledb.Update(sql) > 0;



        }
    }
}
