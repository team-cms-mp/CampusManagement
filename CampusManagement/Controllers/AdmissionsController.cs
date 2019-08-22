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
    public class AdmissionsController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        AdmissionDashbordTemplatesViewModel model = new AdmissionDashbordTemplatesViewModel();

        public ActionResult NewAdmission()
        {
            //Convert.ToInt32(Session["CurrentUserID"]
            model.TemplateList = db.GetAdmissionDashbordTemplates(Convert.ToInt32(Session["CurrentUserID"])).ToList();
            return View(model);
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult ProgramIntake()
        {
            return View();
        }

        public ActionResult SemesterEnrolment()
        {
            return View();
        }


    }
}
