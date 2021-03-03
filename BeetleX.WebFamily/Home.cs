using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.FastHttpApi.Jwt;
using System.Threading.Tasks;

namespace BeetleX.WebFamily
{
    [Controller(BaseUrl = "website")]
    [BeetleX.FastHttpApi.AuthMark(FastHttpApi.AuthMarkType.NoValidation)]
    public class Home
    {

        public async Task<object> LoadInfo(string jwt_user, string jwt_role, IHttpContext context)
        {
            var user = context.GetJwtToken();
            IList<Menu> menus = new List<Menu>();
            if (WebHost.GetMenus != null)
                menus = await WebHost.GetMenus(jwt_user, jwt_role, context);
            return new
            {
                WebHost.MustLogin,
                WebHost.LogoImg,
                WebHost.LoginModel,
                WebHost.Title,
                WebHost.HeaderModel,
                WebHost.HomeModel,
                User = user?.Name,
                Role = user?.Role,
                Menus = menus,
                WebHost.FooterModel
            };
        }

        public void Signout(IHttpContext context)
        {
            context.ClearJwtToken();
        }

        public async Task Login(string Name, string Password, IHttpContext context)
        {
            if (string.IsNullOrEmpty(Name))
                throw new Exception("无效的用户名!");
            if (WebHost.LoginHandler == null)
            {
                context.SetJwtToken(Name, "user", 60 * 60);
            }
            else
            {
                await WebHost.LoginHandler(Name, Password, context);
            }
        }
    }
}
