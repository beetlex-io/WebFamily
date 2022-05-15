using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BeetleX.EFCore.Extension;
using System.Dynamic;

namespace BeetleX.WebFamily.BasicInformation
{
    [Controller(BaseUrl = "baseinfo/permissions")]
    [Authorize(Roles = new string[] { "Admin", "管理员", "系统管理员" })]
    public class PermissionsController
    {
        public object List(string category, EFCoreEntities<IBaseInfoDB> db)
        {
            SqlExpression<Permissions> exp = new SqlExpression<Permissions>();
            if (!string.IsNullOrEmpty(category))
                exp &= p => p.Category == category;
            return db.Entities.Permissions.Where(exp)
                .OrderBy(p => p.Name).OrderBy(p => p.Category).Select(p => p).ToArray();

        }
        public object Get(string id, EFCoreEntities<IBaseInfoDB> db)
        {

            return db.Entities.Permissions.Find(id);
        }
        public SQL2ObjectList<string> ListCategories(EFCoreEntities<IBaseInfoDB> db)
        {
            return (db.DBContext, "select distinct Category from Permissions");
        }
        public void Delete(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            var item = db.Entities.Permissions.Find(id);
            if (item?.SystemData == true)
            {
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            }
            if (item != null)
                db.Entities.Permissions.Remove(item);
        }

        public void Modify(Permissions body, EFCoreEntities<IBaseInfoDB> db)
        {
            if (string.IsNullOrEmpty(body.ID))
            {
                if (db.Entities.Permissions.Where(p => p.Code == body.Code).Count() > 0)
                    ExceptionFactory.PERMISSION_ADD_CODE_EXISTS();
                body.ID = Guid.NewGuid().ToString("N");
                db.Entities.Permissions.Add(body);
            }
            else
            {
                var item = db.Entities.Permissions.Find(body.ID);
                if (item != null)
                {
                    body.EntityCopyOut(item, "SystemData");
                }
            }
        }
    }
}
