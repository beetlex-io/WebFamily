﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi.Jwt;
using BeetleX.FastHttpApi;
using System.Dynamic;

namespace BeetleX.WebFamily.BasicInformation
{
    [Controller(BaseUrl = "baseinfo/properties")]
    [Authorize(Roles = new string[] { "Admin", "管理员", "系统管理员" })]
    public class PropertyController
    {
        public SQL2ObjectList<string> ListCategories(EFCoreEntities<IBaseInfoDB> db)
        {
            return (db.DBContext, "select distinct Category from Properties");
        }

        public object ListSelectOptions(string category, EFCoreEntities<IBaseInfoDB> db)
        {
            return db.Entities.Properties.Where(p => p.Category == category)
                .OrderBy(p => p.Category).OrderBy(p => p.OrderValue)
                .Select(p => new { value = p.Value, label = p.Name }).ToArray();
        }

        public object List(string category, int page, int size, EFCoreEntities<IBaseInfoDB> db)
        {
            SqlExpression<Property> exp = new SqlExpression<Property>();
            if (!string.IsNullOrEmpty(category))
                exp &= p => p.Category == category;
            int count = db.Entities.Properties.Where(exp).Count();
            var items = db.Entities.Properties.Where(exp).OrderBy(p => p.Category)
                .OrderBy(p => p.OrderValue).Select(u => u).Skip(page * size).Take(size);
            return new { items = items.ToArray(), count = count };
        }

        public void Modify(Property body, EFCoreEntities<IBaseInfoDB> db)
        {
            if (string.IsNullOrEmpty(body.ID))
            {

                body.ID = Guid.NewGuid().ToString("N");
                db.Entities.Properties.Add(body);
            }
            else
            {
                var item = db.Entities.Properties.Find(body.ID);
                if (item != null)
                {
                    body.EntityCopyOut(item, "ID");
                }
            }
        }

        public void Delete(string id, EFCoreEntities<IBaseInfoDB> db)
        {
            if (db.Entities.Properties.Where(p => p.ID == id && p.SystemData).Count() > 0)
                ExceptionFactory.DELETE_SYSTEM_DATA_ERROR();
            db.Entities.Properties.Where(p => p.ID == id).Delete();
        }

    }
}
