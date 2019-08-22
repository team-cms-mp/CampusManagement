using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class ScholarshipApplicantsController : Controller
    {
        ScholarshipApplicantViewModel model = new ScholarshipApplicantViewModel();
        // GET: ScholarshipApplicants
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        public ActionResult Index(int? ScholarshipOpportunitiesID)
        {
            ViewBag.hdnScholarshipOpportunitiesID = ScholarshipOpportunitiesID;
            return View();
        }

        public ActionResult LoadScholarShipDetails(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.SO = db.ScholarshipOpportunities.Find(id);
            model.SA = db.ScholarshipApplicants.Where(x => x.ScholarshipOpportunitiesID == id && x.StudentID == 7004).FirstOrDefault();
            model.DocumentList = db.GetScholarshipDocumentByStudentID(id, 7004).ToList();

            ViewBag.ScholarshipOpportunitiesID = id;

            if (model.SO == null)
            {
                return HttpNotFound();
            }

            return View(model);
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