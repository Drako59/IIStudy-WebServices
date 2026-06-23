
using LLStudy_Models.Models;
using LLStudy_Models.ViewModels;
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
        protected ModelCreatorReflection moderlRefCreator;

        public Repository(DbHelperOledb helperOledb, ModelCreatorReflection modelCreatorReflection)
        {
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
        
        }

        
        public virtual bool CreateWithHash(T model, List<string>? exludes = null)
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
                if (pro.Name.Equals($@"{OBJtype.Name}Salt"))
                {
                    this.helperOledb.AddParameter(("@" + pro.Name.ToString()), salt);
                }
                else if (pro.Name.EndsWith("Password"))
                    this.helperOledb.AddParameter(("@" + pro.Name.ToString()), GetHash((string)pro.GetValue(model, null), salt));
                else
                    this.helperOledb.AddParameter(("@" + pro.Name.ToString()), pro.GetValue(model, null) ?? "");



                Console.WriteLine(@$"{"@" + pro.Name}:  {pro.GetValue(model, null)?.ToString() ?? ""} ");
            }

            return this.helperOledb.Insert(sql) > 0;



            

        }

        //public string CreateAndGetId(T model, List<string>? exludes = null)
        //{

        //    Type OBJtype = model.GetType();
        //    List<string> exludePROP = new List<string> { "IsValid", "HasErrors" };
        //    if (exludes != null)
        //        exludePROP.AddRange(exludes);
        //    PropertyInfo[] propertys = OBJtype.GetProperties().Where(p => (!exludePROP.Contains(p.Name) && (!p.Name.Equals(OBJtype.Name + "ID")))).ToArray();
        //    //foreach (PropertyInfo pro in propertys)
        //    //{
        //    //    Console.WriteLine(@$"Property Name: {pro.Name}");
        //    //}
        //    string columns = string.Join(", ", propertys.Select(p => $@"[{p.Name}]"));
        //    string placeholders = string.Join(", ", propertys.Select(p => ("@" + p.Name)));
        //    string sql = $@"INSERT INTO {OBJtype.Name}s ({columns}) VALUES({placeholders})";
        //    Console.WriteLine(sql);
        //    string salt = GetSalt(GetRandomNumber());

        //    Type PropretyType;
        //    string? value;
        //    foreach (PropertyInfo pro in propertys)
        //    {


        //        PropretyType = pro.PropertyType;
        //        //value = Convert.ToString(Convert.ChangeType(pro.GetValue(model, null), PropretyType));
        //        if (pro.Name.Equals($@"{OBJtype.Name}Salt"))
        //        {
        //            this.helperOledb.AddParameter(("@" + pro.Name.ToString()), salt);
        //        }
        //        else if (pro.Name.EndsWith("Password"))
        //            this.helperOledb.AddParameter(("@" + pro.Name.ToString()), GetHash((string)pro.GetValue(model, null), salt));
        //        else
        //            this.helperOledb.AddParameter(("@" + pro.Name.ToString()), pro.GetValue(model, null) ?? "");



        //        Console.WriteLine(@$"{"@" + pro.Name}:  {pro.GetValue(model, null)?.ToString() ?? ""} ");
        //    }
        //    sql += "SELECT @@IDENTITY;";
        //    using (IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        if (reader.Read())
        //            return Convert.ToString(reader[$"{OBJtype.Name}ID"]);
        //    }
        //    return null;
        //}
        public bool CreateFew(List<T> modelList, List<string>? exludes = null)
        {
            Type OBJtype = modelList[0].GetType();
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

            Type PropretyType;
            string? value;
            this.helperOledb.OpenTransaction();
            foreach (T model in modelList)
            {
                foreach (PropertyInfo pro in propertys)
                {


                    PropretyType = pro.PropertyType;
                    //value = Convert.ToString(Convert.ChangeType(pro.GetValue(model, null), PropretyType));
                   this.helperOledb.AddParameter(("@" + pro.Name.ToString()), pro.GetValue(model, null) ?? "");



                    Console.WriteLine(@$"{"@" + pro.Name}:  {pro.GetValue(model, null)?.ToString() ?? ""} ");
                }

                this.helperOledb.Insert(sql);
            }

            this.helperOledb.Commit();

            this.helperOledb.CloseConnection();
            return true;


        }

        public bool CreateFewWithHash(List<T> modelList, List<string>? exludes = null)
        {
            Type OBJtype = modelList[0].GetType();
            List<string> exludePROP = new List<string> { "IsValid", "HasErrors" };
            if (exludes != null)
                exludePROP.AddRange(exludes);
            PropertyInfo[] propertys = OBJtype.GetProperties().Where(p => (!exludePROP.Contains(p.Name) && (!p.Name.Equals(OBJtype.Name + "ID")))).ToArray();
            
            string columns = string.Join(", ", propertys.Select(p => $@"[{p.Name}]"));
            string placeholders = string.Join(", ", propertys.Select(p => ("@" + p.Name)));
            string sql = $@"INSERT INTO {OBJtype.Name}s ({columns}) VALUES({placeholders})";
            Console.WriteLine(sql);
            string salt = GetSalt(GetRandomNumber());

            Type PropretyType;
            string? value;
            this.helperOledb.OpenTransaction();
            foreach (T model in modelList)
            {
                foreach (PropertyInfo pro in propertys)
                {


                    PropretyType = pro.PropertyType;
                    if (pro.Name.Equals($@"{OBJtype.Name}Salt"))
                    {
                        this.helperOledb.AddParameter(("@" + pro.Name.ToString()), salt);
                    }
                    else if (pro.Name.EndsWith("Password"))
                        this.helperOledb.AddParameter(("@" + pro.Name.ToString()), GetHash((string)pro.GetValue(model, null), salt));
                    else
                        this.helperOledb.AddParameter(("@" + pro.Name.ToString()), pro.GetValue(model, null) ?? "");



                }

                this.helperOledb.Insert(sql);
            }

            this.helperOledb.Commit();

            this.helperOledb.CloseConnection();
            return true;


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
            //PropertyInfo prop = typeObj.GetProperty(@$"{typeObj.Name}ID");
            string sql = @$"DELETE * FROM {typeObj.Name}s WHERE {typeObj.Name}ID = @ID";
            Console.WriteLine(sql);
            this.helperOledb.AddParameter("@ID", id);
            return this.helperOledb.Delete(sql) > 0;
        }

        public virtual List<T> GetAll(List<string>? exludes = null)
        {
            List<T> records = new List<T>();

            string Class_name = typeof(T).Name;
            string sql = $@"SELECT * FROM {Class_name}s ORDER BY {Class_name}ID "; //Added ORDER BY
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
            PropertyInfo[] props = typeProp.GetProperties().Where(p => (!exludedProps.Contains(p.Name) && !p.Name.Equals($@"{typeProp.Name}ID"))).ToArray();
            string sets = string.Join(", ", props.Select(p => @$"[{p.Name}] = @{p.Name}"));

            PropertyInfo propID = typeProp.GetProperty(@$"{typeProp.Name}ID");

            sql = $@"UPDATE {typeProp.Name}s SET {sets} WHERE [{propID.Name}] = @{propID.Name}";

            foreach(PropertyInfo pro in props)
            {
                //Console.WriteLine(@$"@{pro.Name} : {pro.GetValue(model, null).GetType()}");
                this.helperOledb.AddParameter(@$"@{pro.Name}", pro.GetValue(model, null) ?? "");
            }

            this.helperOledb.AddParameter($@"@{propID.Name}", propID.GetValue(model, null));
            Console.WriteLine(sql);
            return this.helperOledb.Update(sql) > 0;



        }

        public string GetLastID()
        {

            Type ObjType = typeof(T);
            string sql = "SELECT @@IDENTITY";
            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                    return Convert.ToString(reader[0]);
            }
            return null;

        }

        public List<ViewReview> GetReviewsByBook(string bookID)
        {
            List<ViewReview> reviews = new List<ViewReview>();
            //string sql = "SELECT * FROM Reviews WHERE BookID = @BookID";
            string sql = $@"SELECT   Reviews.RegisteredID AS [RegisteredID],
                                (SELECT COUNT(*) FROM Likes WHERE ReviewID = Reviews.ReviewID AND [IsLike] = True) AS [LikesCount]
                               , (SELECT COUNT(*) FROM Likes WHERE ReviewID = Reviews.ReviewID AND IsDislike = True) AS [Dislikes]   
                               ,* FROM  Reviews
                                    INNER JOIN ( Registereds
                                       
                                    ) ON Registereds.RegisteredID = Reviews.RegisteredID
                                WHERE
                                    BookID = @BookID
                                ORDER BY Reviews.ReviewID DESC";
                                        
            this.helperOledb.AddParameter("@BookID", bookID);

            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                while (reader.Read())
                {
                    reviews.Add(this.moderlRefCreator.CreateModel<ViewReview>(reader));
                }
            }
            return reviews;
        }

        public (Stream, string) GetPdf(string AboslutePath)
        {
            FileStream stream = System.IO.File.OpenRead(AboslutePath);
            string ext = Path.GetExtension(AboslutePath).ToLowerInvariant();
            string contentType = ext switch
            {
                ".pdf" => "application/pdf",
                _ => throw new Exception("Unsupported file type")
            };

            return (stream, contentType);
        }

        public bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
