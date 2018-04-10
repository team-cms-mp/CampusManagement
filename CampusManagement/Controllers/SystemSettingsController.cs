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
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
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
