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
    using System.ComponentModel.DataAnnotations;
    public partial class GetEnrolledStudent_Result
    {
        [Display(Name = "Search By Form #")]
        public int StudentID { get; set; }
        public string FormNo { get; set; }
        public string FullName { get; set; }
        public string ACNIC { get; set; }
        public string CellNo { get; set; }
        public string Email { get; set; }
        public string RollNumber { get; set; }
        public string RegistrationNo { get; set; }
        public string BatchCode { get; set; }
        public string BatchName { get; set; }
        public string BatchSession { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string Picture { get; set; }
        public int BatchProgramID { get; set; }
        [Display(Name = "Semester #")]
        public int YearSemesterNo { get; set; }
        [Display(Name = "Issue Date")]
        public string IssueDate { get; set; }
        [Display(Name = "Last Date")]
        public string LastDate { get; set; }
        [Display(Name = "Account")]
        public string AccountID { get; set; }
    }
}
