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
    
    public partial class ExamGradeAppealForm
    {
        public int ExamGradeAppealFormID { get; set; }
        public Nullable<int> ExamID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public string StudentSignature { get; set; }
        public Nullable<bool> IsStudentSignature { get; set; }
        public string StudentComment { get; set; }
        public Nullable<bool> ApproveComment { get; set; }
        public Nullable<bool> NotApprove { get; set; }
        public string HodComment { get; set; }
        public string HodSignature { get; set; }
        public Nullable<bool> IsHodSignature { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}