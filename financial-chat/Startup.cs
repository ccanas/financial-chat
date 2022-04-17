using financial_chat.business.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalRChat;

[assembly: OwinStartupAttribute(typeof(financial_chat.Startup))]
namespace financial_chat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
                typeof(ChatHub),
                () => new ChatHub(new StockService()));
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
