# BeetleX快速Web开发框架
Beetlex基于前后端分离设计的Web快速开发组件，服务端基于BeetleX.FastHttpApi,前端则以Vuejs为基础核心默认集成element和FontAwesome。在数据交互上集成axios以上前端组件的js和css进了默认集成无须添加。除了集成这些基础功能外，组件还配套了页面布局，菜单和权限登陆功能；在组件的支持下只需要编写vue的业务组件即可装配成成一个单页面的Web应用。

## 用户权限管理模块 (BeetleX.WebFamily.BasicInformation)
- 用户管理
- 部门管理
- 权限配置
- 基础属性配置
- [在线演示](https://beetlex-io.com/systembase/)

## NorthWind示例 (BeetleX.Samples.WebFamily.Northwind)
- 客户管理
- 订单管理
- 分析统计
- [在线演示](https://beetlex-io.com/systembase/)
## 使用
以下是集成用户权限管理模块的启动代码
``` csharp
class Program
{
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            WebHost.Title = "BeetleX用户权限系统";
            WebHost.HeaderModel = "baseinfo-header";
            WebHost.LoginHandler = (user, password, context) =>
            {
                return Task.FromResult(new LoginResult { Role = "admin", Success = true });
            };
            WebHost.GetMenus = (user, role, httpcontext) =>
            {
                return Task.FromResult(Utils.GetMenus());
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
            .UseEFCore<BaseInfoDBContext, MysqlBaseInfoDBContext>()
            .Initialize((http, vue, resoure) =>
            {
                resoure.AddCss("website.css");
                vue.Debug();
            }).Run();
    }
}
```



