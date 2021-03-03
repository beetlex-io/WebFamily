using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi.Hosting;
using BeetleX.FastHttpApi.Jwt;
using BeetleX.FastHttpApi.VueExtend;
using BeetleX.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.WebFamily
{
    public class WebHost : IDisposable
    {

        public WebHost()
        {

        }

        public static string Title { get; set; } = "BeetleX Webfamily";

        public static string LogoImg { get; set; } = "/images/logo.png";

        public static string HeaderModel { get; set; } = "webfamily-heade";

        public static string FooterModel { get; set; }

        public static string LoginModel { get; set; } = "webfamily-login";

        public static string HomeModel { get; set; } = "webfamily-home";

        public static bool MustLogin { get; set; } = false;

        public static EventLogin LoginHandler { get; set; }

        public static EventGetMenus GetMenus { get; set; }

        private List<Assembly> mAssemblies = new List<Assembly>();

        private HostBuilder mHostBuilder = new HostBuilder();

        private Action<HostBuilderContext, IServiceCollection> mConfigureDelegate;

        private Action<HttpOptions> mHttpSetting;

        private Action<HttpApiServer, VueResource, WebResource> mServerInitialize;

        private Action<HttpApiServer> mCompleted;

        private Dictionary<Type, Type> mDbContext = new Dictionary<Type, Type>();

        private Dictionary<Type, ParameterBinder> mRedis = new Dictionary<Type, ParameterBinder>();

        private bool mUseJwt = false;

        private IHost mHost;

        public bool ImportElementUI { get; set; } = true;

        public bool ImportBootstrapIcons { get; set; } = false;

        public bool ImportBootstrap { get; set; } = false;

        public bool ImportFontawesome { get; set; } = false;

        public PageStyle Page { get; set; } = PageStyle.ElementUI;

        public WebHost UseBootstrap(PageStyle style = PageStyle.Bootstrap)
        {
            WebHost.LoginModel = "bootstrap-login";
            ImportElementUI = false;
            ImportBootstrap = true;
            ImportBootstrapIcons = true;
            Page = style;
            return this;
        }

        public WebHost UseBootstrapIcons()
        {
            ImportBootstrapIcons = true;
            return this;
        }

        public WebHost UseFontawesome()
        {
            ImportFontawesome = true;
            return this;
        }

        public WebHost UseElement()
        {
            ImportElementUI = true;
            ImportBootstrap = false;
            ImportBootstrapIcons = false;
            Page = PageStyle.ElementUI;
            WebHost.LoginModel = "webfamily-login";
            return this;
        }

        private void OnInitServer(HttpApiServer server)
        {
            foreach (var item in mDbContext)
            {
                server.ActionFactory.RegisterParameterBinder(item.Value, item.Key);
            }
            foreach (var item in mRedis)
            {
                server.ActionFactory.RegisterParameterBinder(item.Key, item.Value);
            }
            var vue = server.Vue();
            var resource = vue.CreateWebResorce("beetlex");
            if (ImportElementUI)
            {
                resource.AddCss("beetlex-element.css");
                resource.AddCss("beetlex-element-site.css");
            }
            if (ImportBootstrap)
            {
                resource.AddCss("beetlex-bootstrap.css");
                if (Page == PageStyle.Bootstrap)
                    resource.AddCss("beetlex-bootstrap-site.css");
                else
                    resource.AddCss("beetlex-bootstrap-dashboard.css");
            }
            if (ImportBootstrapIcons)
                resource.AddCss("beetlex-bootstrap-icons.css");

            if (ImportFontawesome)
            {
                resource.AddCss("fontawesome.css");
            }

            resource.AddScript("axios.js", "vue.js", "beetlex4axios.js", "vueextends.js", "FileLoader.js");
            if (ImportElementUI)
                resource.AddScript("element.js", "autobase.js");
            if (ImportBootstrap)
                resource.AddScript("bootstrap.js");

            server.ResourceCenter.SetDefaultPages("bootstrap-index.html");
            vue.CssRewrite("/css/{group}-{v}.css").JsRewrite("/js/{group}-{v}.js");
            resource.AddAssemblies(typeof(WebHost).Assembly);
            server.Register(typeof(WebHost).Assembly);
            foreach (var item in mAssemblies)
            {
                server.Register(item);
                resource.AddAssemblies(item);
            }
            switch (Page)
            {
                case PageStyle.Bootstrap:
                    server.ResourceCenter.SetDefaultPages("bootstrap-index.html");

                    break;
                case PageStyle.BootstrapDashboard:
                    server.ResourceCenter.SetDefaultPages("bootstrap-dashboard.html");

                    break;
                default:
                    server.ResourceCenter.SetDefaultPages("index.html");

                    break;
            }

            server.AddExts("ttf;woff;ico;woff2;svg;eot");
            if (mUseJwt)
                server.UseJWT();
        }

        public WebHost UseJWT()
        {
            mUseJwt = true;
            return this;
        }

        public WebHost UseRedis<T>(Action<T> setting)
            where T : RedisDB, new()
        {
            RedisDBParameterBinder<T> item = new RedisDBParameterBinder<T>();
            setting?.Invoke(item.RedisDB);
            mRedis[typeof(T)] = item;
            return this;
        }

        public WebHost UseEFCore<T>()
             where T : DbContext, new()
        {
            mDbContext.Add(typeof(EFCoreContextBinder<T>), typeof(EFCoreDB<T>));
            return this;
        }

        public WebHost RegisterController<T>()
        {
            this.mAssemblies.Add(typeof(T).Assembly);
            return this;
        }

        public WebHost ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            mConfigureDelegate = configureDelegate;
            return this;
        }

        public WebHost Setting(Action<HttpOptions> httpSetting)
        {
            this.mHttpSetting = httpSetting;
            return this;
        }

        public WebHost Initialize(Action<HttpApiServer, VueResource, WebResource> serverInitialize)
        {
            this.mServerInitialize = serverInitialize;
            return this;
        }

        public WebHost Completed(Action<HttpApiServer> completed)
        {
            mCompleted = completed;
            return this;
        }

        public void Run(bool useThread = false)
        {
            mHostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.UseBeetlexHttp(o =>
                {
                    mHttpSetting?.Invoke(o);
                },
                server =>
                {
                    OnInitServer(server);
                    mServerInitialize?.Invoke(server, server.Vue(), server.GetWebFamily());
                },
                completed =>
                {
                    mCompleted?.Invoke(completed);
                }
                );
            });
            mHost = mHostBuilder.Build();
            if (useThread)
            {
                Task.Run(() =>
                {
                    mHost.Run();
                });
            }
            else
            {
                mHost.Run();
            }
        }

        public void Dispose()
        {
            mHost?.StopAsync();
        }
    }

    public static class BeetlexEFCoreExtensions
    {
        public static WebResource GetWebFamily(this HttpApiServer server)
        {
            return server.Vue().CreateWebResorce("beetlex");
        }
    }
}
