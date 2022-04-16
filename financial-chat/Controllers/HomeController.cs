using financial_chat.business.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace financial_chat.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            return View(); 
        }
    }
}