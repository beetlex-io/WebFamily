using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.WebFamily
{
    public class Menu
    {
        public Menu()
        {
            ID = Guid.NewGuid().ToString("N");
        }
        public string ID { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public string Model { get; set; }

        public bool Expend { get; set; } = false;

        public object Data { get; set; }

        public List<Menu> Childs { get; set; } = new List<Menu>();
    }

    public delegate Task<List<Menu>> EventGetMenus(string user, string role, IHttpContext context);

    public delegate Task EventLogin(string user, string pwd, IHttpContext context);
}
