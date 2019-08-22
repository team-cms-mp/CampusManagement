using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static System.Net.Mime.MediaTypeNames;

namespace CampusManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ModelCMSNewContainer dbcms = new ModelCMSNewContainer();
        public static int CampusSettingID = Convert.ToInt32(ConfigurationManager.AppSettings["CampusSettingID"]);
        private static CampusSetting cs = dbcms.CampusSettings.FirstOrDefault(c => c.CampusSettingID == CampusSettingID);
        public static List<CampusHomePageSetting> lstchps = dbcms.CampusHomePageSettings.Where(c => c.CampusSettingID == CampusSettingID).ToList();

        public static int Hospital_ID = cs.Hospital_ID;
        public static string CompanyLogo = cs.CompanyLogo;
        public static string CampusLogo = cs.CampusLogo;
        public static string CampusBackground = cs.CampusBackground;
        public static string CampusName = cs.CampusName;
        public static string LogoWidth = cs.LogoWidth;
        public static string LogoHeight = cs.LogoHeight;
        public static string MarginTop = cs.MarginTop;
        public static string CampusTitle = cs.CampusTitle;
        public static string MarginLeftTitle = cs.MarginLeftTitle;
        public static string EligibilityPercentage = cs.EligibilityPercentage;
        public static string CampusAddress = cs.CampusAddress;
        public static string UniversityCampusName = cs.UniversityCampusName;
        public static string UniversityName = cs.UniversityName;
        public static string LayoutName = cs.LayoutName;
        public static string PhoneNumber1 = cs.PhoneNumber1;
        public static string PhoneNumber2 = cs.PhoneNumber2;
        public static string PhoneNumber3 = cs.PhoneNumber3;
        public static string MobileNumber1 = cs.MobileNumber1;
        public static string MobileNumber2 = cs.MobileNumber2;
        public static string MobileNumber3 = cs.MobileNumber3;
        public static string UniversityEmail = cs.UniversityEmail;
        public static string CampusEmail = cs.CampusEmail;
        public static string QueryEmail = cs.QueryEmail;
        public static string Extension1 = cs.Extension1;
        public static string Extension2 = cs.Extension2;
        public static string Extension3 = cs.Extension3;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
