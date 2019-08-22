using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class ScholarshipListDisplaysController : Controller
    {
        // GET: ScholarshipListDisplays
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        public ActionResult Index()
        {
            return View(db.ScholarshipOpportunities.OrderByDescending(a => a.ScholarshipOpportunitiesID));
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