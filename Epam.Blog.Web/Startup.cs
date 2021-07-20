using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Epam.Blog.Web.Startup))]
namespace Epam.Blog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
