# BeetleX Web SPA组件
Beetlex基于前后端分离设计的Web SPA快速开发组件，服务端基于BeetleX.FastHttpApi,前端则以Vuejs为基础核心默认集成element和bootstrap5主题，在数据交互上集成axios以上前端组件的js和css进了默认集成无须添加。除了集成这些基础功能外，组件还配套了页面布局，菜单和权限登陆功能；在组件的支持下只需要编写vue的业务组件即可以配置完成一个SPA的Web应用。

<img src="https://user-images.githubusercontent.com/2564178/109756382-cde4fa80-7c22-11eb-966b-408978876708.png" height="16">[组件示例](https://github.com/beetlex-io/BeetleX-Samples)
<img src="https://user-images.githubusercontent.com/2564178/109802889-4f587f00-7c5b-11eb-85f4-f262cf7800c0.png" height="16">[使用介绍](http://blog.beetlex.io/?postid=212afc979b8e4f3b9e2b7dd7aac664b3)
## 使用
引用组件最新版本BeetleX.WebFamily,在控制台项目中编写以下代码
``` csharp
class Program
{
    static void Main(string[] args)
    {
        WebHost host = new WebHost();
        host.RegisterController<Program>()
        .Setting(o =>
        {
            o.SetDebug();
            o.Port = 80;
            o.LogLevel = EventArgs.LogType.Info;
            o.LogToConsole = true;
        })
        .UseFontawesome()//加入Fontawesome
        .UseBootstrap(PageStyle.Bootstrap)//设置使用bootstrap
        .Initialize((http, vue, resoure) =>
        {
            vue.Debug();
        }).Run();
    }
}
```

## 项目应用效果
![image](https://user-images.githubusercontent.com/2564178/109803826-7c596180-7c5c-11eb-973a-188fca6e970c.png)
![image](https://user-images.githubusercontent.com/2564178/109804003-b7f42b80-7c5c-11eb-90cb-44dc1c718ee7.png)
![image](https://user-images.githubusercontent.com/2564178/109804122-dc500800-7c5c-11eb-8bd0-6817c536e00c.png)


