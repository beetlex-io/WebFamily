using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi.EFCore.Extension;
using System.Dynamic;
using System.Linq;

namespace BeetleX.WebFamily.BasicInformation
{

    [Controller(BaseUrl = "baseinfo/users")]
    [Authorize(Roles = new string[] { "Admin", "管理员", "系统管理员" })]
    public class UserController
    {
        public object List(string matchName, string department, int page, int size, EFCoreEntities<IBaseInfoDB> db)
        {

            SQL sql = @"select 
                        a.ID,
                        a.Name,
                        a.WorkNumber,
                        a.MobilePhone,
                        a.EMail,
                        a.JobPostion,
                        b.name Department,
                        a.Enabled,
                        a.SystemData,
                        c.name Supperior
                        from users a left join departments b on
                        a.DepartmentID= b.id left join users c
                        on a.SuperiorID = c.id where 1=1";
            if (!string.IsNullOrEmpty(matchName))
                sql += (" and a.name like @p1", "@p1", $"%{matchName}%");
            if (!string.IsNullOrEmpty(department))
                sql += (" and a.DepartmentID = @p2", "@p2", department);
            int count = sql.Count(db.DBContext);
            var items = sql.List<ExpandoObject>(db.DBContext, new Region(page, size));
            return new { items, count };
        }

        public SQL2ObjectList<ExpandoObject> ListSelectOptions(EFCoreEntities<IBaseInfoDB> db)
        {
            SQL sql = @"select 
                        a.ID value,
                        a.WorkNumber,
                        a.Name label,
                        a.JobPostion,
                        b.name Department
                        from users a left join departments b on
                        a.DepartmentID= b.id order by a.Name";
            return (db.DBContext, sql);
        }

        public object GetPermission(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            return BaseInfoUtils.GetUserPermissionAggres(id, db.Entities);
        }

        public void Delete(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            var user = db.Entities.Users.Find(id);
            if (user?.SystemData == true)
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            if (user != null)
                db.Entities.Users.Remove(user);
            DataCached.RefreshUsers(db.Entities);
        }

        public object Get(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            return db.Entities.Users.Find(id);
        }

        public void Modify(User body, EFCoreEntities<IBaseInfoDB> db)
        {
            body.EMail = body.EMail.ToLower();
            if (string.IsNullOrEmpty(body.ID))
            {
                body.ID = Guid.NewGuid().ToString("N");
                if (db.Entities.Users.Where(user => user.EMail == body.EMail).Count() > 0)
                    ExceptionFactory.USER_ADD_EMAIL_EXISTS();
                if (db.Entities.Users.Where(user => user.WorkNumber == body.WorkNumber).Count() > 0)
                    ExceptionFactory.USER_ADD_WORKNUMBER_EXISTS();
                body.LoginPassword = BaseInfoUtils.HashPassword(BaseInfoUtils.DefaultPassword);
                db.Entities.Users.Add(body);
            }
            else
            {
                if (db.Entities.Users.Where(user => user.EMail == body.EMail
                && user.ID != body.ID
                ).Count() > 0)
                    ExceptionFactory.USER_ADD_EMAIL_EXISTS();

                if (db.Entities.Users.Where(
                    user => user.WorkNumber == body.WorkNumber
                    && user.ID != body.ID).Count() > 0)
                    ExceptionFactory.USER_ADD_WORKNUMBER_EXISTS();
                var user = db.Entities.Users.Find(body.ID);
                if (user != null)
                {
                    body.EntityCopyOut(user, "ID", "SystemData", "LoginPassword", "Enabled");
                }
            }
            DataCached.RefreshUsers(db.Entities);
        }

        public void Enabled(string id, bool enabled, EFCoreEntities<IBaseInfoDB> db)
        {
            var user = db.Entities.Users.Find(id);
            if (user?.SystemData == true)
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            if (user != null)
                user.Enabled = enabled;
        }

        public void ChangePassword(string id, string password, EFCoreEntities<IBaseInfoDB> db)
        {
            password = BaseInfoUtils.HashPassword(password);
            var user = db.Entities.Users.Find(id);
            if (user != null)
                user.LoginPassword = password;
        }
    }
}
