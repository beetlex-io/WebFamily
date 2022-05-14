using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BeetleX.EFCore.Extension;
using System.Dynamic;

namespace BeetleX.WebFamily.BasicInformation
{
    public class Utils
    {
        public static string DefaultPassword { get; set; } = "123456";
        public static Func<string, string> HashPasswordHandler { get; set; }
        public static string HashPassword(string password)
        {
            if (HashPasswordHandler != null)
                return HashPasswordHandler(password);
            return BeetleX.FastHttpApi.Utils.EncryptToSHA1(password);
        }

        public static Dictionary<string, bool> GetUserPermission(string userid, BaseInfoDBContext db)
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

        public static List<UserPermissionAggre> GetUserPermissionAggres(string userid, BaseInfoDBContext db)
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
            item.Img = "fa-solid fa-building-user";
            item.Name = "部门管理";
            item.Model = "baseinfo-department-list";
            result.Add(item);

            item = new Menu();
            item.ID = "users";
            item.Img = "fa-solid fa-user-group";
            item.Name = "员工管理";
            item.Model = "baseinfo-user-list";
            result.Add(item);


            item = new Menu();
            item.ID = "roles";
            item.Img = "fa-solid fa-people-group";
            item.Name = "系统角色";
            item.Model = "baseinfo-roles-list";
            result.Add(item);

            item = new Menu();
            item.ID = "permission";
            item.Img = "fa-solid fa-sliders";
            item.Name = "功能权限";
            item.Model = "baseinfo-permission-list";
            result.Add(item);

            item = new Menu();
            item.ID = "properties";
            item.Img = "fa-solid fa-database";
            item.Name = "基础数据";
            item.Model = "baseinfo-properties-list";
            result.Add(item);

            return result;
        }
    }
}
