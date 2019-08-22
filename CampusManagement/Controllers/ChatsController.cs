using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class ChatsController : Controller
    {
        // GET: Chats
        public ActionResult Chat()
        {
            return View();
        }
    }
}