using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusManagement.Models;
using Newtonsoft.Json;
using PagedList;
using System.Globalization;

using CampusManagement.App_Code;
using System.Configuration;
using System.IO;
using System.Net.Mail;


namespace CampusManagement.Controllers
{
    public class ObeProgramPLOController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        OBE_ProgramPLO ModelObj = new OBE_ProgramPLO();
        //GetAllOBE_ProgramPLO_Result model = new GetAllOBE_ProgramPLO_Result();

        // GET: ObeProgramPLO
        [HttpGet]
        public ActionResult Index(int? page, int? pageSize ,string ProgramID, string PLOName, string PLOCode, string message)
        {
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.pageSize = pageSize;
            int pageNumber = (page ?? 1);

            if (ProgramID == null)
            {
                ProgramID = "0";
            }
            if (PLOName == null  || PLOName == "")
            {
                PLOName = "";
            }

            if (PLOCode == null || PLOCode == "")
            {
                PLOCode = "";
            }
            
            ViewBag.ProgramID = new SelectList(GetProgramslist(), "ProgramID", "ProgramName", ProgramID);
            ViewBag.hdnProgramID = ProgramID;
            ViewBag.hdnPLOName = PLOName;
            ViewBag.hdnPLOCode = PLOCode;
           // model.ProgramPloList = db.GetAllOBE_ProgramPLO(Convert.ToInt32(ProgramID), PLOCode, PLOName, "0").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            return View(db.GetAllOBE_ProgramPLO(Convert.ToInt32(ProgramID), PLOCode, PLOName, "").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult Index(int? page, string ProgramID, string PLOName, string PLOCode)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (ProgramID == null)
            {
                ProgramID = "0";
            }
            if (PLOName == null || PLOName == "")
            {
                PLOName = "";
            }

            if (PLOCode == null || PLOCode == "")
            {
                PLOCode = "";
            }
            ViewBag.hdnProgramID = ProgramID;
           ViewBag.hdnPLOName = PLOName;
           ViewBag.hdnPLOCode = PLOCode;
           ViewBag.ProgramID = new SelectList(GetProgramslist(), "ProgramID", "ProgramName", ProgramID);
          // model.ProgramPloList = db.GetAllOBE_ProgramPLO(Convert.ToInt32(ProgramID), PLOCode, PLOName, "0").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize));
            return View(db.GetAllOBE_ProgramPLO(Convert.ToInt32(ProgramID), PLOCode, PLOName, "").ToList().ToPagedList(pageNumber, Convert.ToInt32(pageSize)));

        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ProgramID = new SelectList(GetProgramsWithOutAll(), "ProgramID", "ProgramName");
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            return View(ModelObj);
        }

        [HttpPost]
        public ActionResult Create(OBE_ProgramPLO obj)
        {
            ViewBag.ProgramID = new SelectList(GetProgramsWithOutAll(), "ProgramID", "ProgramName", obj.ProgramID);
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", obj.IsActive);

            int PLOID =  Convert.ToInt32(db.InsertOrUpdateOBE_ProgramPLO(obj.PLOID,
            obj.ProgramID,
            obj.PLOCode,
            obj.PLOName,
            obj.PLOTypeID,
            obj.Description,
            null,
            Convert.ToInt32(Session["emp_id"]),
            obj.IsActive,
            null,
            Convert.ToInt32(Session["emp_id"])).FirstOrDefault());
            if (PLOID > 0)
            {
                ViewBag.MessageType = "success";
                ViewBag.Message = "Data has been saved successfully.";
            }
            else {
                ViewBag.MessageType = "error";
                ViewBag.Message = "Error while executing query, please try again";
            }

            return View(obj);
        }


        public JsonResult GetPrograms()
        {
            List<Program> lstPrograms = new List<Program>();
            Program ProgramtObj = new Program();
            ProgramtObj.ProgramID = 0;
            ProgramtObj.ProgramName = "--ALL--";
            lstPrograms = db.Programs.ToList();
            lstPrograms.Insert(0, ProgramtObj);
            var programs = lstPrograms.Select(p => new
            {
                ProgramID = p.ProgramID,
                ProgramName = p.ProgramName
            });
            string result = JsonConvert.SerializeObject(programs, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<Program> GetProgramslist()
        {
            List<Program> lstPrograms = new List<Program>();
            Program ProgramtObj = new Program();
            ProgramtObj.ProgramID = 0;
            ProgramtObj.ProgramName = "--ALL--";
            lstPrograms = db.Programs.ToList();
            lstPrograms.Insert(0, ProgramtObj);
            //var programs = lstPrograms.Select(p => new
            //{
            //    ProgramID = p.ProgramID,
            //    ProgramName = p.ProgramName
            //});
            
            return lstPrograms;
        }

        public List<Program> GetProgramsWithOutAll()
        {
            List<Program> lstPrograms = new List<Program>();
            lstPrograms = db.Programs.ToList();
            return lstPrograms;
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
