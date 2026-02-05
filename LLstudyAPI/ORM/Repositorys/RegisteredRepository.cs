using LLStudy_Models.Models;
using Microsoft.AspNetCore.Mvc;

using LLstudyWS.ORM.CreatorsModels;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

using System.IO;


namespace LLstudyWS.ORM
{
    public class RegisteredRepository : Repository<Registered>, IRepository<Registered>
    {
        public RegisteredRepository(DbHelperOledb helper, ModelCreators modelCreator, ModelCreatorReflection modelCretorRef) : base(helper, modelCreator, modelCretorRef) { }

        public Registered Login( string password, string? signTool = null)
            
        {
            
            string sql = "SELECT * FROM Registereds WHERE UserName = @SignTool OR Email = @SignTool";
            
            this.helperOledb.AddParameter("@SignTool", signTool);

            


            using (IDataReader reader = this.helperOledb.Select(sql)) 
            {
                if (reader.Read())
                {
                    if (GetHash(password, Convert.ToString(reader["RegisteredSalt"])) == Convert.ToString(reader["Password"]))
                        return this.moderlRefCreator.CreateModel<Registered>(reader, new List<string>() {"Password", "IsValid", "HasErrors"});

                }
                    //return this.moderlRefCreator.CreateModel<Registered>(reader, new List<string>() { "Password" });
            }
            return null;


        }

        public string LoginID(string password,  string? SignKey = null)
        {
            string sql = "SELECT * FROM Registereds WHERE UserName = @SignKey OR Email = @SignKey ";

            this.helperOledb.AddParameter("@SignKey", SignKey);

           


            using (IDataReader reader = this.helperOledb.Select(sql))
            {
                if (reader.Read())
                {
                    string salt = reader["RegisteredSalt"].ToString();
                    string hash = reader["Password"].ToString();
                    string CalHash = GetHash(password, salt);
                    if ( CalHash == hash)
                    {
                        return Convert.ToString(reader["RegisteredID"]);

                    }
                    Console.WriteLine("Here");
                    return null;
                }
            }
            return null;
        }

        public (Stream,string) GetImage(string AboslutePath)
        {
            FileStream stream = System.IO.File.OpenRead(AboslutePath);
            //string contentType = "application/octet-stream";
            string ext = Path.GetExtension(AboslutePath).ToLowerInvariant();
            string contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
            //IFormFile formFile = new FormFile(stream, 0, stream.Length, null, imageName)
            //{
            //    Headers = new HeaderDictionary(),
            //    ContentType = contentType
            //};
            return (stream, contentType);
        }

        public string ChangeImage(IFormFile file, string registeredID)
        {

            //if (!HttpContext.Request.Form.Files.Any())
            //    return false;
            //IFormFile image = HttpContext.Request.Form.Files[0];
            if (file == null || file.Length == 0)
                throw new Exception("Empty file");

            //string path = Path.Combine(Directory.GetCurrentDirectory()!, "App_Data","RegisteredsImages");
            string path = Path.Combine(Directory.GetCurrentDirectory()!, "wwwroot", "Images", "RegisteredImages");

            Directory.CreateDirectory(path);

            string ext = Path.GetExtension(file.FileName);
            Console.WriteLine($"FileName = '{file.FileName}', ContentType = '{file.ContentType}'");

            if (string.IsNullOrEmpty(ext))
            {
                ext = (file.ContentType ?? "").ToLowerInvariant() switch
                {
                    "image/jpeg" => ".jpg",
                    "image/png" => ".png",
                    "image/gif" => ".gif",
                    _ => throw new Exception("Unsupported file type")
                };
            }

            string fileName = $"User{registeredID}{ext}";

            path = Path.Combine(path, fileName);
            Console.WriteLine("********************************" + path);


            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }



            return fileName;
        }
        //public override Registered GetByID(string UserName)
        //{
        //    string sql = "SELECT * FROM Registereds WHERE UserName = @UserName";

        //    this.helperOledb.AddParameter("@UserName", UserName);

        //    Registered obj;
        //    using(IDataReader reader = this.helperOledb.Select(sql))
        //    {
        //        if (reader.Read())
        //        {
        //            obj = this.moderlRefCreator.CreateModel<Registered>(reader);
        //            return obj;
        //        }
        //    }
        //    return new Registered();
        //}
    }
}
