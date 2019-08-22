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
    public class ProvincesController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        ProvincesViewModel model = new ProvincesViewModel();

        public ActionResult Index()
        {
            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Province province)
        {
            try
            {
                Province p = db.Provinces.FirstOrDefault(prov => prov.ProvinceName == province.ProvinceName);
                if (p == null)
                {
                    province.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    province.CreatedOn = DateTime.Now;
                    db.Provinces.Add(province);
                    try
                    {
                        db.SaveChanges();
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                    }
                    catch (DbUpdateException ex)
                    {
                        ViewBag.MessageType = "error";
                        ViewBag.Message = ex.Message;
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Province Name already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "Province Name already exists.";
                }
            }
            catch (DbEntityValidationException ex)
            {
                string ErrorMessage = "";
                int count = 0;
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += count + "-" + string.Concat(error.PropertyName, " is required.") + "<br />";
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", province.IsActive);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", province.CountryID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GetProvinces_Result province = db.GetProvinces("").FirstOrDefault(x=>x.ProvinceID == id);
            if (province == null)
            {
                return HttpNotFound();
            }

            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = province;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", province.IsActive);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", province.CountryID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Province province)
        {
            try
            {
                db.Entry(province).State = EntityState.Modified;
                province.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                province.ModifiedOn = DateTime.Now;
                try
                {
                    db.SaveChanges();
                    ViewBag.MessageType = "success";
                    ViewBag.Message = "Data has been saved successfully.";
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.MessageType = "error";
                    ViewBag.Message = ex.Message;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            catch (DbEntityValidationException ex)
            {
                string ErrorMessage = "";
                int count = 0;
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        count++;
                        ErrorMessage += string.Concat(count, "-", error.ErrorMessage, "\n");
                    }
                }
                ViewBag.MessageType = "error";
                ViewBag.Message = ErrorMessage;
            }
            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", province.IsActive);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", province.CountryID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GetProvinces_Result province = db.GetProvinces("").FirstOrDefault(x => x.ProvinceID == id);
            if (province == null)
            {
                return HttpNotFound();
            }

            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = province;
            model.DisplayMode = "Delete";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Province province = db.Provinces.Find(id);
                db.Provinces.Remove(province);
                db.SaveChanges();
                ViewBag.MessageType = "success";
                ViewBag.Message = "Record has been removed successfully.";
            }
            catch (DbUpdateException ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            model.Provinces = db.GetProvinces("").OrderByDescending(a => a.ProvinceID).ToList();
            model.SelectedProvince = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");

            return View("Index", model);
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
