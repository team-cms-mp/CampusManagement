//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CampusManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CampusSetting
    {
        public int CampusSettingID { get; set; }
        public string CampusName { get; set; }
        public string CampusLogo { get; set; }
        public string CampusBackground { get; set; }
        public string CompanyLogo { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string LogoWidth { get; set; }
        public string LogoHeight { get; set; }
    }
}
