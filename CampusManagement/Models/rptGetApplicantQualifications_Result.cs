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
    
    public partial class rptGetApplicantQualifications_Result
    {
        public string FormNo { get; set; }
        public string ApplicantName { get; set; }
        public int DegreeID { get; set; }
        public string DegreeName { get; set; }
        public int YearQualification { get; set; }
        public double TotalMarks { get; set; }
        public double ObtainedMarks { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<int> InstituteID { get; set; }
        public string InstituteName { get; set; }
    }
}
