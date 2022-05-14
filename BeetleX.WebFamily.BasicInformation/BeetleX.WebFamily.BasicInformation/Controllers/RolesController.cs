using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.EFCore.Extension;
using System.Linq;
using System.Dynamic;

namespace BeetleX.WebFamily.BasicInformation.Controllers
{
    [Controller(BaseUrl = "baseinfo/roles")]
    [Authorize(Roles = new string[] { "Admin", "管理员", "系统管理员" })]
    public class RolesController
    {
        public object List(string matchName, int page, int size, EFCoreDB<BaseInfoDBContext> db)
        {
            SqlExpression<Role> exp = new SqlExpression<Role>();
            if (!string.IsNullOrEmpty(matchName))
                exp &= r => r.Name.Contains(matchName);
            var count = db.DBContext.Roles.Where(exp).Count();
            var items = db.DBContext.Roles.Where(exp).Select(r => r).Skip(page * size).Take(size).ToArray();
            return new { items, count };
        }

        public SQL2ObjectList<ExpandoObject> ListSelectOptions(EFCoreDB<BaseInfoDBContext> db) {

            return (db.DBContext, "select ID value,Name label,Note from Roles");
        }

        public object Get(string id, EFCoreDB<BaseInfoDBContext> db)
        {
            return db.DBContext.Roles.Find(id);
        }

        public class PermissionAggre
        {
            public string Category { get; set; }
            public List<AggrsItem> Items { get; set; } = new List<AggrsItem>();

            public class AggrsItem
            {

                public string RoleID { get; set; }

                public string Name { get; set; }

                public string Code { get; set; }

                public string PermissionsID { get; set; }

                public bool Value { get; set; }
            }
        }

        public void ModifyPermission(string id, List<PermissionAggre> items, EFCoreDB<BaseInfoDBContext> db)
        {
            var rolePermission = db.DBContext.RolePermissions.Where(p => p.RoleID == id).ToArray();
            foreach (var category in items)
            {
                foreach (var item in category.Items)
                {
                    var rec = rolePermission.FirstOrDefault(p => p.PermissionsID == item.PermissionsID);
                    if (rec != null)
                    {
                        rec.Value = item.Value ? 1 : 0;
                    }
                    else
                    {
                        rec = new RolePermissions();
                        rec.RoleID = id;
                        rec.PermissionsID = item.PermissionsID;
                        rec.Value = item.Value ? 1 : 0;
                        db.DBContext.RolePermissions.Add(rec);
                    }
                }
            }
        }
        public object ListPermission(string id, EFCoreDB<BaseInfoDBContext> db)
        {
            var permission = db.DBContext.Permissions.OrderBy(p => p.Category).OrderBy(p => p.Name).ToArray();
            var rolePermission = db.DBContext.RolePermissions.Where(p => p.RoleID == id).ToArray();

            Dictionary<string, PermissionAggre> result = new Dictionary<string, PermissionAggre>();
            foreach (var item in permission)
            {
                if (!result.TryGetValue(item.Category, out PermissionAggre aggrs))
                {
                    aggrs = new PermissionAggre { Category = item.Category };
                    result[item.Category] = aggrs;
                }
                bool value = false;
                var ritem = rolePermission.FirstOrDefault(p => p.PermissionsID == item.ID);
                if (ritem != null)
                    value = ritem.Value > 0;
                aggrs.Items.Add(new PermissionAggre.AggrsItem
                {
                    RoleID = id,
                    Name = item.Name,
                    Code = item.Code,
                    PermissionsID = item.ID,
                    Value = value
                });
            }
            return result.Values;
        }

        public void Delete(string id, EFCoreDB<BaseInfoDBContext> db)
        {
            var item = db.DBContext.Roles.Find(id);
            if (item?.SystemData == true)
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            if (item != null)
                db.DBContext.Roles.Remove(item);
        }

        public void Modify(Role body, EFCoreDB<BaseInfoDBContext> db)
        {
            if (string.IsNullOrEmpty(body.ID))
            {
                if (db.DBContext.Roles.Where(r => r.Name == body.Name).Count() > 0)
                    ExceptionFactory.ROLS_ADD_NAME_EXISTS();
                body.ID = Guid.NewGuid().ToString("N");
                db.DBContext.Roles.Add(body);
            }
            else
            {
                var role = db.DBContext.Roles.Find(body.ID);
                if (role != null)
                {
                    body.EntityCopyOut(role, "ID", "SystemData");
                }
            }
        }
    }
}
