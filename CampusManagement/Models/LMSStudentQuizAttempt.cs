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
    
    public partial class LMSStudentQuizAttempt
    {
        public int LMSStudentQuizAttemptID { get; set; }
        public int LMSQuizID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<bool> IsNegativeMarking { get; set; }
        public Nullable<int> StudentAttempts { get; set; }
        public Nullable<System.DateTime> AttemptStartTime { get; set; }
        public Nullable<System.DateTime> AttemptEndTime { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }

    public class LMSStudentQuizAttemptViewModel
    {
        public List<LMSStudentQuizAttempt> LMSStudentQuizAttempts { get; set; }
        public LMSStudentQuizAttempt SelectedLMSStudentQuizAttempts { get; set; }
        public string DisplayMode { get; set; }
    }
}