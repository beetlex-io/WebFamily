using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BeetleX.EFCore.Extension;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using BeetleX.FastHttpApi;

namespace BeetleX.WebFamily.BasicInformation
{
    [BeetleX.FastHttpApi.Controller]
    public class InitialieController : BeetleX.FastHttpApi.IController
    {
        [NotAction]
        public void Init(HttpApiServer server, string path)
        {
            BaseInfoUtils.Initialize?.Invoke(server);
        }
    }

    public class DataCached
    {
        private static Dictionary<string, User> mUsers = new Dictionary<string, User>();

        private static Dictionary<string, Department> mDepartments = new Dictionary<string, Department>();

        public static Dictionary<string, Department> GetDepartments(IBaseInfoDB db)
        {
            lock (mDepartments)
            {
                if (mDepartments.Count == 0)
                {
                    foreach (var item in db.Departments)
                    {
                        mDepartments[item.ID] = item;
                    }
                }
                return mDepartments;
            }
        }

        public static void RefreshDepartments(IBaseInfoDB db)
        {
            lock (mDepartments)
            {
                foreach (var item in db.Departments)
                {
                    mDepartments[item.ID] = item;
                }
            }
        }

        public static string GetUserNames(IBaseInfoDB db, params string[] id)
        {
            List<string> result = new List<string>();
            var users = GetUsers(db);
            foreach (var item in id)
            {
                if (users.TryGetValue(item, out User user))
                {
                    result.Add(user.Name);
                }
            }
            return string.Join(',', result.ToArray());
        }

        public static Dictionary<string, User> GetUsers(IBaseInfoDB db)
        {
            lock (mUsers)
            {
                if (mUsers.Count == 0)
                {

                    foreach (var item in db.Users)
                    {
                        mUsers[item.ID] = item;
                    }

                }
                return mUsers;
            }
        }

        public static void RefreshUsers(IBaseInfoDB db)
        {
            lock (mUsers)
            {

                foreach (var item in db.Users)
                {
                    mUsers[item.ID] = item;
                }

            }
        }
    }


    public class BaseInfoUtils
    {
        public static string DefaultPassword { get; set; } = "123456";
        public static Func<string, string> HashPasswordHandler { get; set; }

        public static Action<HttpApiServer> Initialize { get; set; }

        public static User Login(IBaseInfoDB db, string email, string password)
        {
            password = HashPassword(password);
            var user = db.Users.Where(p => p.EMail == email.ToLower() && p.LoginPassword == password && p.Enabled).FirstOrDefault();
            return user;
        }

        public static void CreateAdmin(string email, IBaseInfoDB db)
        {
            var user = db.Users.FirstOrDefault(u => u.EMail == email.ToLower());
            if (user == null)
            {
                user = new User();
                user.ID = Guid.NewGuid().ToString("N");
                user.Name = "管理员";
                user.IsAdmin = true;
                user.EMail = email.ToLower();
                user.LoginPassword = HashPassword(DefaultPassword);
                user.SystemData = true;
                user.Enabled = true;
                db.Users.Add(user);
                ((DbContext)db).SaveChanges();
            }
        }

        public static string HashPassword(string password)
        {
            if (HashPasswordHandler != null)
                return HashPasswordHandler(password);
            return BeetleX.FastHttpApi.Utils.EncryptToSHA1(password);
        }

        public static Dictionary<string, bool> GetUserPermission(string userid, IBaseInfoDB db)
        {
            var items = GetUserPermissionAggres(userid, db);
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (var item in items)
            {
                foreach (var sitem in item.Items)
                {
                    result[sitem.Code] = sitem.Value;
                }
            }
            return result;
        }

        public static List<UserPermissionAggre> GetUserPermissionAggres(string userid, IBaseInfoDB db)
        {

            var user = db.Users.Find(userid);
            var permission = db.Permissions.OrderBy(p => p.Category).OrderBy(p => p.Name).ToArray();
            var values = from a in new string[0]
                         select new { Key = "", Value = 0 };
            if (user == null || string.IsNullOrEmpty(user.Roles))
            {
                values = from a in db.RolePermissions
                         where false
                         group a by a.PermissionsID into g
                         select new { g.Key, Value = g.Max(p => p.Value) };
            }
            else
            {
                var roleid = user.Roles.Split(',');
                values = from a in db.RolePermissions
                         where roleid.Contains(a.RoleID)
                         group a by a.PermissionsID into g
                         select new { g.Key, Value = g.Max(p => p.Value) };
            }

            Dictionary<string, UserPermissionAggre> result = new Dictionary<string, UserPermissionAggre>();
            foreach (var item in permission)
            {
                if (!result.TryGetValue(item.Category, out UserPermissionAggre aggrs))
                {
                    aggrs = new UserPermissionAggre { Category = item.Category };
                    result[item.Category] = aggrs;
                }
                bool value = false;
                var ritem = values.FirstOrDefault(p => p.Key == item.ID);
                if (ritem != null)
                    value = ritem.Value > 0;
                aggrs.Items.Add(new UserPermissionAggre.AggrsItem
                {
                    Name = item.Name,
                    Code = item.Code,
                    PermissionsID = item.ID,
                    Value = value
                });
            }
            return result.Values.ToList();
        }

        public static List<Menu> GetMenus()
        {

            List<Menu> result = new List<Menu>();
            var item = new Menu();
            item.ID = "department";
            item.Icon = "fa-solid fa-building-user";
            item.Name = "部门管理";
            item.Model = "baseinfo-department-list";
            result.Add(item);

            item = new Menu();
            item.ID = "users";
            item.Icon = "fa-solid fa-user-group";
            item.Name = "员工管理";
            item.Model = "baseinfo-user-list";
            result.Add(item);


            item = new Menu();
            item.ID = "roles";
            item.Icon = "fa-solid fa-people-group";
            item.Name = "系统角色";
            item.Model = "baseinfo-roles-list";
            result.Add(item);

            item = new Menu();
            item.ID = "permission";
            item.Icon = "fa-solid fa-sliders";
            item.Name = "功能权限";
            item.Model = "baseinfo-permission-list";
            result.Add(item);

            item = new Menu();
            item.ID = "properties";
            item.Icon = "fa-solid fa-database";
            item.Name = "基础数据";
            item.Model = "baseinfo-properties-list";
            result.Add(item);

            return result;
        }
    }
}
