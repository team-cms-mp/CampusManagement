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
using Newtonsoft.Json;

namespace CampusManagement.Controllers
{
    [Authorize(Roles = "Account Officer,Accounts Officer,Admin Assistant,Admin Officer,Admin.Assistant,Assist. Account Officer,Assist.Technician,Import Manager,Manager Servive & Support,Office Manager,Officer QMS,RSM - Center 2,RSM - South,Sales & Service Executive,Sales Executive,Sales Manager,Sales Representative,Sr.Accounts Officer,Sr.Associate Engineer,Sr.Sales Executive,Sr.Sales Representative,Store Assistant,Store Incharge,Technician")]
    public class CitiesController : Controller
    {
        private ModelCMSContainer db = new ModelCMSContainer();
        CitiesViewModel model = new CitiesViewModel();

        public ActionResult Index()
        {
            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(City city)
        {
            try
            {
                City c = db.Cities.FirstOrDefault(cit => cit.CityName == city.CityName);
                if (c == null)
                {
                    city.CreatedBy = Convert.ToInt32(Session["emp_id"]);
                    city.CreatedOn = DateTime.Now;
                    db.Cities.Add(city);
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
                    ModelState.AddModelError(string.Empty, "City Name is already exists.");
                    ViewBag.MessageType = "error";
                    ViewBag.Message = "City Name is already exists.";
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
            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", city.IsActive);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", city.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", city.ProvinceID);

            return View("Index", model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }

            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = city;
            model.DisplayMode = "ReadWrite";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", city.IsActive);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", city.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", city.ProvinceID);
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(City city)
        {
            try
            {
                db.Entry(city).State = EntityState.Modified;
                city.ModifiedBy = Convert.ToInt32(Session["emp_id"]);
                city.ModifiedOn = DateTime.Now;
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
            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", city.IsActive);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", city.CountryID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", city.ProvinceID);

            return View("Index", model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }

            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = city;
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
                City city = db.Cities.Find(id);
                db.Cities.Remove(city);
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
            model.Cities = db.Cities.OrderByDescending(a=>a.CityID).ToList();
            model.SelectedCity = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName");

            return View("Index", model);
        }

        public JsonResult GetProvinceList(string CountryID)
        {
            List<Province> lstProvince = new List<Province>();
            int cId = Convert.ToInt32(CountryID);

            lstProvince = db.Provinces.Where(p => p.CountryID == cId).ToList();
            var Provinces = lstProvince.Select(S => new {
                ProvinceID = S.ProvinceID,
                ProvinceName = S.ProvinceName,
            });
            string result = JsonConvert.SerializeObject(Provinces, Formatting.Indented);
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
