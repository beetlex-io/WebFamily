using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using BeetleX.FastHttpApi.EFCore.Extension;

namespace BeetleX.HelpViewer
{
    [FastHttpApi.Controller(BaseUrl = "admin")]
    [FastHttpApi.Jwt.Authorize(Roles = new string[] { "admin" })]
    public class Admin
    {
        public object ListUsers(string matchName, int page, EFCoreDB<HelpViewerDB> db)
        {
            SqlExpression<User> exp = new SqlExpression<User>();
            if (!string.IsNullOrEmpty(matchName))
                exp &= (u) => u.Name.Contains(matchName);
            var count = db.DBContext.Users.Where(exp).Count();
            var items = db.DBContext.Users.Where(exp).Select(a => a).Skip(page * 20).Take(20).ToArray();
            return new { count, items };
        }

        public void UserEnabled(string id, bool enabled, EFCoreDB<HelpViewerDB> db)
        {
            db.DBContext.Users.Where(u => u.ID == id).Update(u => new User { Enabled = enabled });
        }

        public void UserAdminEnabled(string id, bool enabled, EFCoreDB<HelpViewerDB> db)
        {
            db.DBContext.Users.Where(u => u.ID == id).Update(u => new User { IsAdmin = enabled });
        }

        public void UserChangePwd(string id, string password, EFCoreDB<HelpViewerDB> db)
        {
            password = BeetleX.FastHttpApi.Utils.EncryptToSHA1(password);
            db.DBContext.Users.Where(u => u.ID == id).Update(u => new User { Password = password });
        }
        [SkipFilter(typeof(FastHttpApi.Jwt.AuthorizeAttribute))]
        public SQL2ObjectList<ExpandoObject> FolderList(EFCoreDB<HelpViewerDB> db)
        {
            SQL sql = "select *,(select count(*) from Documents where Documents.Folder=Folders.ID) Total from Folders";
            return (db.DBContext, sql);
        }

        public void FolderDelete(string id, EFCoreDB<HelpViewerDB> db)
        {
            db.DBContext.Folders.Where(f => f.ID == id).Delete();
        }

        public void FolderAdd(Folder body, EFCoreDB<HelpViewerDB> db)
        {
            body.ID = Guid.NewGuid().ToString("N");
            db.DBContext.Folders.Add(body);
        }

        public void DocumentChangeOrderValue(string id, int value, EFCoreDB<HelpViewerDB> db)
        {
            db.DBContext.Documents.Where(p => p.ID == id).Update(d => new Document { OrderValue = value });
        }

        [SkipFilter(typeof(FastHttpApi.Jwt.AuthorizeAttribute))]
        public object DocumentList(string folder, int page, string jwt_role, EFCoreDB<HelpViewerDB> db)
        {
            SQL sql = @"select Documents.ID,Documents.Title,Documents.OrderValue,
Documents.Tag,strftime('%Y-%m-%d', Documents.CreateTime) CreateTime,Folders.Name FolderName from Documents left join Folders on Documents.Folder= Folders.ID where 1=1 ";
            if (!string.IsNullOrEmpty(folder))
            {
                sql += " and Documents.Folder=@p1";
                sql += ("@p1", folder);
            }
            if (jwt_role.ToLower() != "admin")
            {
                sql += " and Documents.Enabled > 0";
            }

            if (string.IsNullOrEmpty(folder))
            {
                sql.OrderByDESC("Documents.CreateTime");

            }
            else
            {
                sql.OrderByASC("Documents.OrderValue");
            }
            var count = sql.Count(db.DBContext);
            var items = sql.List<ExpandoObject>(db.DBContext, new Region(page, 20));
            return new { count, items };
        }
        private string GetFilder(string id, HelpViewerDB db)
        {
            return db.Folders.Find(id)?.Name;
        }
        public void DocumentDelete(string id, EFCoreDB<HelpViewerDB> db)
        {
            db.DBContext.Documents.Where(d => d.ID == id).Delete();

        }
        [SkipFilter(typeof(FastHttpApi.Jwt.AuthorizeAttribute))]
        public Document DocumentGet(string id, EFCoreDB<HelpViewerDB> db)
        {

            return db.DBContext.Documents.Find(id);
        }

        public void DocumentModify(Document body, EFCoreDB<HelpViewerDB> db)
        {
            if (string.IsNullOrEmpty(body.ID))
            {
                body.ID = Guid.NewGuid().ToString("N");
                body.CreateTime = DateTime.Now;
                body.OrderValue = 0;
                db.DBContext.Documents.Add(body);

            }
            else
            {
                var doc = db.DBContext.Documents.Find(body.ID);
                if (doc != null)
                {
                    body.EntityCopyOut(doc, "OrderValue");
                }
            }
        }
        [ThreadStatic]
        private static byte[] mReadFileBuffer;
        public object UploadImageUrl(string url, IHttpContext context)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                string folder = context.Server.Options.StaticResourcePath + System.IO.Path.DirectorySeparatorChar + "files" + System.IO.Path.DirectorySeparatorChar;
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.Directory.CreateDirectory(folder);
                    for (int i = 0; i < 10; i++)
                    {
                        var subfolder = folder + $"{i:00}";
                        if (!System.IO.Directory.Exists(subfolder))
                            System.IO.Directory.CreateDirectory(subfolder);
                    }
                }
                string filename = $"{Guid.NewGuid().ToString("N")}.jpg";
                string fullName = $"{folder}{Math.Abs(filename.GetHashCode() % 10):00}{System.IO.Path.DirectorySeparatorChar}{filename}";
                using (System.IO.Stream stream = System.IO.File.Create(fullName))
                {
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
                return new TextResult($"/files/{Math.Abs(filename.GetHashCode() % 10):00}/{filename}");
            }
        }
        [Put]
        [NoDataConvert]
        public object UploadFile(string name, bool isimage, IHttpContext context)
        {
            string folder = context.Server.Options.StaticResourcePath + System.IO.Path.DirectorySeparatorChar + "files" + System.IO.Path.DirectorySeparatorChar;
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
                for (int i = 0; i < 10; i++)
                {
                    var subfolder = folder + $"{i:00}";
                    if (!System.IO.Directory.Exists(subfolder))
                        System.IO.Directory.CreateDirectory(subfolder);
                }
            }
            string filename = $"{Guid.NewGuid().ToString("N")}{System.IO.Path.GetExtension(name)}";
            string fullName = $"{folder}{Math.Abs(filename.GetHashCode() % 10):00}{System.IO.Path.DirectorySeparatorChar}{filename}";
            if (mReadFileBuffer == null)
                mReadFileBuffer = new byte[1024 * 4];
            using (System.IO.Stream stream = System.IO.File.Create(fullName))
            {
                while (context.Request.Stream.Length > 0)
                {
                    var len = context.Request.Stream.Read(mReadFileBuffer, 0, mReadFileBuffer.Length);
                    stream.Write(mReadFileBuffer, 0, len);
                }
                stream.Flush();
            }
            return new TextResult($"/files/{Math.Abs(filename.GetHashCode() % 10):00}/{filename}");
        }
    }

    [FastHttpApi.Controller]
    public class InitController : IController
    {
        [NotAction]
        public void Init(HttpApiServer server, string path)
        {
            using (HelpViewerDB db = new HelpViewerDB())
            {
                string name = "admin";
                var user = db.Users.Where(u => u.Name == name).FirstOrDefault();
                if (user == null)
                {
                    user = new User();
                    user.ID = Guid.NewGuid().ToString("N");
                    user.Name = name;
                    user.Password = Utils.EncryptToSHA1("123456");
                    user.Type = "system";
                    user.IsAdmin = true;
                    user.Enabled = true;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
    }
}
