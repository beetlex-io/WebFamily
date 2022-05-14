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
        public object List(string matchName, string department, int page, int size, EFCoreDB<BaseInfoDBContext> db)
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

        public SQL2ObjectList<ExpandoObject> ListSelectOptions(EFCoreDB<BaseInfoDBContext> db)
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

        public object GetPermission(string id, EFCoreDB<BaseInfoDBContext> db)
        {
            return Utils.GetUserPermissionAggres(id, db.DBContext);
        }

        public void Delete(string id, EFCoreDB<BaseInfoDBContext> db)
        {
            var user = db.DBContext.Users.Find(id);
            if (user?.SystemData == true)
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            if (user != null)
                db.DBContext.Users.Remove(user);
        }

        public object Get(string id, EFCoreDB<BaseInfoDBContext> db)
        {
            return db.DBContext.Users.Find(id);
        }

        public void Modify(User body, EFCoreDB<BaseInfoDBContext> db)
        {
            if (string.IsNullOrEmpty(body.ID))
            {
                if (db.DBContext.Users.Where(user => user.EMail == body.EMail).Count() > 0)
                    ExceptionFactory.USER_ADD_EMAIL_EXISTS();
                if (db.DBContext.Users.Where(user => user.WorkNumber == body.WorkNumber).Count() > 0)
                    ExceptionFactory.USER_ADD_WORKNUMBER_EXISTS();
                body.ID = Guid.NewGuid().ToString("N");
                body.LoginPassword = Utils.HashPassword(Utils.DefaultPassword);
                db.DBContext.Users.Add(body);
            }
            else
            {
                if (db.DBContext.Users.Where(user => user.EMail == body.EMail
                && user.ID != body.ID
                ).Count() > 0)
                    ExceptionFactory.USER_ADD_EMAIL_EXISTS();

                if (db.DBContext.Users.Where(
                    user => user.WorkNumber == body.WorkNumber
                    && user.ID != body.ID).Count() > 0)
                    ExceptionFactory.USER_ADD_WORKNUMBER_EXISTS();
                var user = db.DBContext.Users.Find(body.ID);
                if (user != null)
                {
                    body.EntityCopyOut(user, "ID", "SystemData", "LoginPassword", "Enabled");
                }
            }
        }

        public void Enabled(string id, bool enabled, EFCoreDB<BaseInfoDBContext> db)
        {
            var user = db.DBContext.Users.Find(id);
            if (user?.SystemData == true)
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            if (user != null)
                user.Enabled = enabled;
        }

        public void ChangePassword(string id, string password, EFCoreDB<BaseInfoDBContext> db)
        {
            password = Utils.HashPassword(password);
            var user = db.DBContext.Users.Find(id);
            if (user != null)
                user.LoginPassword = password;
        }
    }
}
