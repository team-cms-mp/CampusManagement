using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;

namespace CampusManagement.Controllers
{
    [Authorize]
    public class SystemSettingsController : Controller
    {
        public ActionResult Academic()
        {
            return View();
        }

        public ActionResult Administration()
        {
            return View();
        }

        public ActionResult Discipline()
        {
            return View();
        }
        public ActionResult ExtraCurriculum()
        {
            return View();
        }
        public ActionResult Finance()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult Campus()
        {
            return View();
        }
    }
}
