using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TopPost.MVC.Startup))]
namespace TopPost.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
