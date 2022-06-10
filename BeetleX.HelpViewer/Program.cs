using BeetleX.WebFamily;
using System;
using BeetleX.FastHttpApi.EFCore.Extension;
using System.Threading.Tasks;
using System.Linq;

namespace BeetleX.HelpViewer
{
    class Program
    {
        static void Main(string[] args)
        {




            WebHost host = new WebHost();
            WebHost.Title = "BeetleX帮助文档";
            WebHost.TabsEnabled = false;
            WebHost.HomeModel = "helpviewer-home";
            WebHost.HomeName = "首页";

            //WebHost.QQConfig.AppID = "";
            //WebHost.QQConfig.AppSecret = "";
            //WebHost.QQConfig.Redirect_uri = "http://test.beetlex-io.com/platformlogin.html?type=QQ";

            //WebHost.GithubConfig.AppID = "";
            //WebHost.GithubConfig.AppSecret = "";
            //WebHost.GithubConfig.Redirect_uri = "http://test.beetlex-io.com/platformlogin.html?type=Github";
            //WebHost.GithubConfig.Token = "";

            //WebHost.MicrosoftConfig.auth.clientId = "";
            //WebHost.MicrosoftConfig.auth.redirectUri = "http://localhost/platformlogin.html";

            WebHost.GetUserInfo = (id) =>
            {
                using (HelpViewerDB db = new HelpViewerDB())
                {
                    var user = db.Users.Find(id);
                    return new { user?.Name, user?.Type };
                }
            };

            WebHost.LoginHandler = (user, pwd, context) =>
            {
                user = user.ToLower();
                using (HelpViewerDB db = new HelpViewerDB())
                {
                    var u = db.Users.Where(u => u.Name == user).FirstOrDefault();
                    if (u != null)
                    {
                        if (u.Password == BeetleX.FastHttpApi.Utils.EncryptToSHA1(pwd))
                        {
                            return Task.FromResult(new LoginResult(u.ID, u.IsAdmin ? "admin" : "user"));
                        }
                    }
                }
                return Task.FromResult(new LoginResult("用户名或密码不匹配!"));
            };
            WebHost.OtherPlatformLogin = (openid, type, contenxt) =>
            {
                string role = "user";
                using (HelpViewerDB db = new HelpViewerDB())
                {
                    var user = db.Users.Where(u => u.Name == openid.ToLower() && u.Type == type.ToString()).FirstOrDefault();
                    if (user == null)
                    {
                        user = new User();
                        user.ID = Guid.NewGuid().ToString("N");
                        user.Name = openid.ToLower();
                        user.Password = FastHttpApi.Utils.EncryptToSHA1(openid);
                        user.Type = type.ToString();
                        user.Enabled = true;
                        user.IsAdmin = false;
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (user.IsAdmin)
                            role = "admin";
                    }
                    return Task.FromResult(new LoginResult(user.ID, role));
                }
            };
            host.RegisterComponent<Program>();
            host.Setting(o =>
            {
                o.LogToConsole = true;
                o.LogLevel = EventArgs.LogType.Warring;
                o.Port = 80;
                o.SetDebug();
                o.MaxBodyLength = 1024 * 1024 * 10;
            })
            .UseJWT()
            .UseEFCore<HelpViewerDB>()
            .UseElement()
            .Initialize((http, vue, resource) =>
            {
                string dbfile = "BeetleX.HelpViewer.helpviewer.db";
                var stream = typeof(Program).Assembly.GetManifestResourceStream(dbfile);

                var pagefile = "helpviewer.db";

                if (!System.IO.File.Exists(pagefile))
                {
                    using (System.IO.Stream fileStream = System.IO.File.Open(pagefile, System.IO.FileMode.Create))
                    {
                        stream.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }

                resource.AddCss("vs.css", "website.css");
                resource.AddScript("exif.js", "highlight.js", "marked.js");
                vue.Debug();
            })
            .Run();
        }
    }
}
