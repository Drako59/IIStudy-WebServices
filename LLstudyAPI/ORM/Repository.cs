
using System.Reflection;

namespace LLstudyWS.ORM
{
    public class Repository<T>: IRepository<T>
    {
        protected DbHelperOledb helperOledb;
        protected ModelCreators modelCreators;

        public Repository()
        {
            this.modelCreators = new ModelCreators();
            this.helperOledb = new DbHelperOledb();
        }

        public virtual bool Create(T model)
        {
            string sql2 = $@"INSERT INTO Books (
                            author_name,
                            book_name,
                            book_price,
                            In_stock,
                            subjectID,
                            pdf_url_book
                            )
                        VALUES
                            (@AuthorName, @BookName, @BookPrice, @InStock, @SubjectID, @PdfUrlBook) ";
            Type OBJtype = model.GetType();
            PropertyInfo[] propertys = OBJtype.GetProperties();

            string columns = string.Join(", ", propertys.Select(p => p.Name));
            string placeholders = string.Join(", ", propertys.Select(p => ("@" + p.Name)));

            string sql = $@"INSERT INTO {OBJtype.Name} ({columns}) VALUES({placeholders})";

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

            Type PropretyType;
            string? value;
            foreach (PropertyInfo pro in propertys)
            {
                PropretyType = pro.PropertyType;
                value = Convert.ToString(Convert.ChangeType(pro.GetValue(model, null), PropretyType));
                this.helperOledb.AddParameter(("@" + pro.Name), (string)value);
            }
            //this.helperOledb.AddParameter("@AuthorName", model.Author_name);
            //this.helperOledb.AddParameter("@BookName", model.Book_name);
            //this.helperOledb.AddParameter("@BookPrice", model.Book_price.ToString());
            //this.helperOledb.AddParameter("@InStock", model.In_stock.ToString());
            //this.helperOledb.AddParameter("@SubjectID", model.SubjectID);
            //this.helperOledb.AddParameter("@PdfUrlBook", model.Pdf_url_book);
            return this.helperOledb.Insert(sql) > 0;
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetByID(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
