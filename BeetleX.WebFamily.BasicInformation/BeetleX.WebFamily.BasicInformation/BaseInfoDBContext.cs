using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    public class BaseInfoDBContext : DbContext
    {


        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<Permissions> Permissions { get; set; }

        public DbSet<RolePermissions> RolePermissions { get; set; }

        private string[] StringToArray(string v)
        {
            if (string.IsNullOrEmpty(v))
                return new string[0];
            return v.Split(',');
        }

        private string ArrayToString(string[] v)
        {
            if (v == null)
                return "";
            else
                return string.Join(',', v);
        }


        private bool IntToBool(int value)
        {
            return value > 0;
        }

        private int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //     modelBuilder
            //.Entity<Permissions>()
            //.Property(e => e.Values)
            //.HasConversion(
            //    v => ArrayToString(v),
            //    v => StringToArray(v));

            //     modelBuilder.Entity<User>()
            //         .Property(e => e.Enabled)
            //         .HasConversion(
            //             v => BoolToInt(v),
            //             v => IntToBool(v)
            //         );


            //     modelBuilder.Entity<User>()
            //        .Property(e => e.SystemData)
            //        .HasConversion(
            //            v => BoolToInt(v),
            //             v => IntToBool(v)
            //        );
            modelBuilder.Entity<RolePermissions>()
                .HasKey(c => new { c.RoleID, c.PermissionsID });
        }

    }
}
