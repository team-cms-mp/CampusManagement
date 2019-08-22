using CampusManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusManagement.Controllers
{
    public class AlumniController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();

        GetAlumni_ResultsViewModel model = new GetAlumni_ResultsViewModel();
        AlumniUserViewModel modelAlumni = new AlumniUserViewModel();
        string ErrorMessage = "";
        int count = 0;

        // GET: Alumni
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(GetAlumni_Result alumi)
        {
            if (alumi.FirstName == null)
            {
                alumi.FirstName = "";
            }
            if (alumi.LastName == null)
            {
                alumi.LastName = "";

            }
            model.GetAlumni_Results = db.GetAlumni(alumi.FirstName, alumi.LastName, alumi.CityID, alumi.ProvinceID, alumi.CountryID).OrderBy(a => a.AlumniUserID).ToList();
            model.SelectedGetAlumni_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult AlumniDirectory()
        {
            model.GetAlumni_Results = db.GetAlumni("", "", 0, 0, 0).ToList();
            model.SelectedGetAlumni_Result = null;
            model.DisplayMode = "WriteOnly";
            ViewBag.cityID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Provincename");
            ViewBag.MessageType = "";
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult AlumniAccountAdmin(int? AlumniUserID)
        {     
            modelAlumni.AlumniUsers = db.AlumniUsers.Where(x => x.AlumniUserID == AlumniUserID).ToList();
            modelAlumni.SelectedAlumniUsers = null;
            modelAlumni.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.AlumniUserID = AlumniUserID;
            ViewBag.Message = "";

            return View(modelAlumni);
        }

        public ActionResult AlumniAccount()
        {
            int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
            // modelAlumni.AlumniUsers = db.AlumniUsers.Where(x => x.AlumniUserID == CurrentUserID).ToList();
            modelAlumni.AlumniUsers = db.AlumniUsers.Where(x => x.CreatedBy == CurrentUserID).ToList();
            modelAlumni.SelectedAlumniUsers = null;
            modelAlumni.DisplayMode = "WriteOnly";
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.MessageType = "";
            ViewBag.AlumniUserID = CurrentUserID;
            ViewBag.Message = "";

            return View(modelAlumni);
        }

        public ActionResult Create()
        {
            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName");
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName");
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName");
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName");
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName");
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniUser alumniUser)
        {

            AlumniUser app = db.AlumniUsers.FirstOrDefault(a => a.AlumniUserID == alumniUser.AlumniUserID);
            if (app == null)
            {
                try
                {
                    if (alumniUser.ProfilePicture != null)
                    {
                        alumniUser.Picture = string.Concat("~/ProfilePics/", alumniUser.AlumniUserID, "_", alumniUser.ProfilePicture.FileName.Replace(" ", ""));
                        alumniUser.ProfilePicture.SaveAs(Server.MapPath(alumniUser.Picture));
                        Session["ProfilePicture"] = alumniUser.ProfilePicture;
                    }
                    else if (Session["ProfilePicture"] != null)
                    {
                        alumniUser.ProfilePicture = (HttpPostedFileBase)Session["ProfilePicture"];
                        alumniUser.Picture = string.Concat("~/ProfilePics/", alumniUser.AlumniUserID, "_", alumniUser.ProfilePicture.FileName);
                        //alumniUser.ProfilePicture.SaveAs(Server.MapPath("~/ProfilePics") + "/" + string.Concat(alumniUser.AlumniUserID, "_", alumniUser.ProfilePicture.FileName));
                    }
                    else
                    {
                        count++;
                        ErrorMessage += count + "-Profile picture is required.<br />";
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Profile picture is required.");
                    count++;
                    ErrorMessage += count + "-Profile picture is required.<br />";
                }

                try
                {
                    alumniUser.CreatedOn = DateTime.Now;
                    alumniUser.CreatedBy = Convert.ToInt32(db.um_GetMaxEmployeeID().FirstOrDefault());
                    db.AlumniUsers.Add(alumniUser);

                    try
                    {
                        if (string.IsNullOrEmpty(ErrorMessage))
                        {
                            db.SaveChanges();
                            ViewBag.MessageType = "success";
                            ViewBag.Message = "Data has been saved successfully.";

                            //Add AlumniUser Login
                            Login login = new Login();
                            login.EmpID = Convert.ToInt32(alumniUser.CreatedBy);
                            login.CNIC = alumniUser.UserCNIC;
                            login.Email = alumniUser.Email;
                            login.UserName = alumniUser.Email;
                            login.Password = "123";
                            login.MobileNumber = alumniUser.CellNo;
                            login.EmpType = "AlumniUser";
                            db.Logins.Add(login);
                            db.SaveChanges();
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            ViewBag.MessageType = "error";
                            ViewBag.Message = ErrorMessage;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        ViewBag.MessageType = "success";
                        ViewBag.Message = "Data has been saved successfully.";
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                catch (DbEntityValidationException ex)
                {
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
            }


            ViewBag.IsActive = new SelectList(db.Options, "OptionDesc", "OptionDesc", alumniUser.IsActive);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", alumniUser.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", alumniUser.CountryID);
            ViewBag.CurrentOccupationID = new SelectList(db.CurrentOccupations, "CurrentOccupationID", "CurrentOccupationName", alumniUser.CurrentOccupationID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "GenderName", alumniUser.GenderID);
            ViewBag.MaritalStatusID = new SelectList(db.MaritalStatus, "MaritalStatusID", "MaritalStatusName", alumniUser.MaritalStatusID);
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "NationalityName", alumniUser.NationalityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "ProvinceName", alumniUser.ProvinceID);
            ViewBag.RelationTypeID = new SelectList(db.RelationTypes, "RelationTypeID", "RelationTypeName", alumniUser.RelationTypeID);
            ViewBag.ReligionID = new SelectList(db.Religions, "ReligionID", "ReligionName", alumniUser.ReligionID);
            ViewBag.SalutationID = new SelectList(db.Salutations, "SalutationID", "SalutationName", alumniUser.SalutationID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", alumniUser.StatusID);
            ViewBag.AlumniUserID = alumniUser.AlumniUserID;
            ViewBag.hdnProvinceID = alumniUser.ProvinceID;
            ViewBag.hdnCityID = alumniUser.CityID;
            return View(alumniUser);
        }
        public JsonResult GetProvinceList(string CountryID)
        {
            List<Province> lstProvince = new List<Province>();
            int cId = Convert.ToInt32(CountryID);

            lstProvince = db.Provinces.Where(p => p.CountryID == cId).ToList();
            var Provinces = lstProvince.Select(S => new
            {
                ProvinceID = S.ProvinceID,
                ProvinceName = S.ProvinceName,
            });
            string result = JsonConvert.SerializeObject(Provinces, Formatting.Indented);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCitiesList(string ProvinceID)
        {
            List<City> lstCity = new List<City>();
            int pId = Convert.ToInt32(ProvinceID);

            lstCity = db.Cities.Where(p => p.ProvinceID == pId).ToList();
            var Cities = lstCity.Select(S => new
            {
                CityID = S.CityID,
                CityName = S.CityName,
            });
            string result = JsonConvert.SerializeObject(Cities, Formatting.Indented);
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