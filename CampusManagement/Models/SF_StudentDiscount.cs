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
    
    public partial class SF_StudentDiscount
    {
        public int StudentDiscountID { get; set; }
        public Nullable<int> DiscountTypeID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> ApplicantID { get; set; }
        public string PercentageInstallmentValue { get; set; }
        public Nullable<int> YearSemesterNo { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public string IsActive { get; set; }
    }
}