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
        private static ModelCMSContainer dbcms = new ModelCMSContainer();
        public static int CampusSettingID = Convert.ToInt32(ConfigurationManager.AppSettings["CampusSettingID"]);
        private static CampusSetting cs = dbcms.CampusSettings.FirstOrDefault(c => c.CampusSettingID == CampusSettingID);
        public static List<CampusHomePageSetting> lstchps = dbcms.CampusHomePageSettings.Where(c => c.CampusSettingID == CampusSettingID).ToList();

        public static string CompanyLogo = cs.CompanyLogo;
        public static string CampusLogo = cs.CampusLogo;
        public static string CampusBackground = cs.CampusBackground;
        public static string CampusName = cs.CampusName;
        public static string LogoWidth = cs.LogoWidth;
        public static string LogoHeight = cs.LogoHeight;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
