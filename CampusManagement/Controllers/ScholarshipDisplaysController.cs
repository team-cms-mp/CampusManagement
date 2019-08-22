using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class ScholarshipDisplaysController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScholarshipOpportunitie scholarshipOpportunitie = db.ScholarshipOpportunities.Find(id);
            if (scholarshipOpportunitie == null)
            {
                return HttpNotFound();
            }

            return View(scholarshipOpportunitie);
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