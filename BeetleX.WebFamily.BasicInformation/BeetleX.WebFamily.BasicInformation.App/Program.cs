using BeetleX.FastHttpApi.EFCore.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BeetleX.WebFamily.BasicInformation.App
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            WebHost.Title = "BeetleX用户权限系统";
            WebHost.HeaderModel = "baseinfo-header";
            WebHost.Login = true;
            WebHost.LoginHandler = (user, password, context) =>
            {
                return Task.FromResult(new LoginResult { Role = "admin", Success = true });
            };
            WebHost.GetMenus = (user, role, httpcontext) =>
            {

                return Task.FromResult(BaseInfoUtils.GetMenus());
            };
            host.RegisterComponent<Program>()
            .RegisterComponent<User>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Error;
                o.LogToConsole = true;
            })
            .UseJWT()
            .UseElement()
            .UseEFCoreEntities<IBaseInfoDB, MysqlBaseInfoDBContext>()
            .Initialize((http, vue, resoure) =>
            {
                resoure.AddCss("website.css");
                vue.Debug();
            }).Run();
        }
    }

    public class MysqlBaseInfoDBContext : DbContext, IBaseInfoDB
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RolePermissions>()
      .HasKey(c => new { c.RoleID, c.PermissionsID });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var connectionString = "server=192.168.2.19;user=root;password=123456;database=baseinfo_test";
            //var connectionString = "server=localhost;user=root;password=henry-0128;database=baseinfo_test";
            var serverVersion = new MySqlServerVersion(new Version(5, 7, 38));
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}
