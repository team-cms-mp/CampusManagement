using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class UserChatController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ChatUserViewModel model = new ChatUserViewModel();
        // GET: UserChat
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserChat(int? ChatWithEmpID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            model.AdminList = db.GetChatUser(1, "9,3").ToList();
            model.LecturarList = db.GetChatUser(2, "31,59").ToList();
            model.StudentList = db.GetChatUser(3, "0,0").ToList();
            ViewBag.CurrentUserEmpID = EmpID;
            if (ChatWithEmpID == null)
            {
                ViewBag.ChatWithUserEmpID = 0;
            }
            else {
                ViewBag.ChatWithUserEmpID = ChatWithEmpID;
            }
            
            return View(model);
        }
      
        public JsonResult LoadChat(string ChatWithUserID, string ChatMessage, string UserChatID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            int? CurrentUserEmpID = EmpID;
            ViewBag.CurrentUserEmpID = EmpID;
            if (Convert.ToInt32(ChatWithUserID) == 0)
            {
                ViewBag.ChatWithUserEmpID = 0;
            }
            else
            {
                ViewBag.ChatWithUserEmpID = Convert.ToInt32(ChatWithUserID);
            }
            db.InsertUserChatMessages(Convert.ToInt32(UserChatID), CurrentUserEmpID, Convert.ToInt32(ChatWithUserID), CurrentUserEmpID, CurrentUserEmpID, CurrentUserEmpID, Convert.ToInt32(ChatWithUserID), ChatMessage);
            List<GetUserChatByUserID_Result> UserChatList = new List<GetUserChatByUserID_Result>();
            UserChatList = db.GetUserChatByUserID(CurrentUserEmpID, Convert.ToInt32(ChatWithUserID)).ToList();
            string result = JsonConvert.SerializeObject(UserChatList, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadChatWithInterval(string ChatWithUserID)
        {
            int EmpID = Convert.ToInt32(Session["emp_id"]);
            int? CurrentUserEmpID = EmpID;
            ViewBag.CurrentUserEmpID = EmpID;
            if (Convert.ToInt32(ChatWithUserID) == 0)
            {
                ViewBag.ChatWithUserEmpID = 0;
            }
            else
            {
                ViewBag.ChatWithUserEmpID = Convert.ToInt32(ChatWithUserID);
            }
            List<GetUserChatByUserID_Result> UserChatList = new List<GetUserChatByUserID_Result>();
            UserChatList = db.GetUserChatByUserID(CurrentUserEmpID, Convert.ToInt32(ChatWithUserID)).ToList();
            string result = JsonConvert.SerializeObject(UserChatList, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}