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

    public partial class SF_GetStudentFines_Result
    {
        public int StudentFineID { get; set; }
        public Nullable<int> CollegeServiceID { get; set; }
        public Nullable<int> YearSemesterNo { get; set; }
        public int StudentFineTypeID { get; set; }
        public string StudentFineTypeName { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string StatusName { get; set; }
        public string CollegeServiceName { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
	
	public class SF_GetStudentFines_ResultViewModel
    {
        public List<SF_GetStudentFines_Result> SF_GetStudentFines_Results { get; set; }
        public SF_GetStudentFines_Result SelectedSF_GetStudentFines_Result { get; set; }
        public string DisplayMode { get; set; }
    }
}