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

    public partial class SP_DegreeTitleForPage_Result
    {
        public int DegreeTitleID { get; set; }
        public string DegreeTitleName { get; set; }
        public int DegreeID { get; set; }
        public string DegreeName { get; set; }
        public string IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string CB_Username { get; set; }
        public string MB_Username { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
     
    }
    public class DegreeTitleViewModel
    {
        public List<SP_DegreeTitleForPage_Result> DegreeTitles { get; set; }
        public SP_DegreeTitleForPage_Result SelectedDegreeTitle { get; set; }
        public string DisplayMode { get; set; }
    }
}
