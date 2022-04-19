using financial_chat.business;
using System.Collections.Generic;
using System.Linq;
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
            List<Chatroom> messages;
            using (var context = new Entities())
            {
                messages = context.Chatrooms.Select(x => x).OrderByDescending(x => x.CreatedDate).Take(50).ToList();
            }
            
            return View(messages); 
        }
    }
}