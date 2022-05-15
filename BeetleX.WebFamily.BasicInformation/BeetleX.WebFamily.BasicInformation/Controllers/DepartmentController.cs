using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Dynamic;

namespace BeetleX.WebFamily.BasicInformation
{
    [Controller(BaseUrl = "baseinfo/department")]
    [Authorize(Roles = new string[] { "Admin", "管理员", "系统管理员" })]
    public class DepartmentController
    {
        public Object List(string matchName, int page, int size, EFCoreEntities<IBaseInfoDB> db)
        {
            SqlExpression<Department> exp = new SqlExpression<Department>();
            if (!string.IsNullOrEmpty(matchName))
                exp &= p => p.Name.Contains(matchName);

            SQL sql = @"select a.*,b.name Manager,c.name Superior from departments a left join users b on
                        a.managerid = b.id left join departments c on a.superiorid = c.id where 1=1";
            if (!string.IsNullOrEmpty(matchName))
            {
                sql += " and a.name like @matchname";
                sql.Parameter("@matchname", "%" + matchName + "%");
            }
            int count = sql.Count(db.DBContext);
            var items = sql.List<ExpandoObject>(db.DBContext, new Region(page, size));
            return new { items = items, count = count };
        }
        public SQL2ObjectList<ExpandoObject> ListSelectOptions(EFCoreEntities<IBaseInfoDB> db)
        {

            return (db.DBContext, "select id value,name label from departments");
        }

        public object Get(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            return db.Entities.Departments.Find(id);
        }

        public void Modify(Department body, EFCoreEntities<IBaseInfoDB> db)
        {
            if (string.IsNullOrEmpty(body.ID))
            {
                body.ID = Guid.NewGuid().ToString("N");
                db.Entities.Departments.Add(body);
            }
            else
            {
                var item = db.Entities.Departments.Find(body.ID);
                if (item != null)
                {
                    body.EntityCopyOut(item, "ID", "SystemData");
                }
            }

        }

        public void Delete(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            var item = db.Entities.Departments.Find(id);
            if (item != null)
            {
                if (item.SystemData)
                    ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
                db.Entities.Departments.Remove(item);
            }
        }
    }
}
