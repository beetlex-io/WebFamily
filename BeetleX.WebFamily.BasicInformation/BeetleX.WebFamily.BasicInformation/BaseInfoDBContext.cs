using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    public interface IBaseInfoDB : IDisposable
    {

        DbSet<User> Users { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<Property> Properties { get; set; }

        DbSet<Permissions> Permissions { get; set; }

        DbSet<RolePermissions> RolePermissions { get; set; }

    }
}
