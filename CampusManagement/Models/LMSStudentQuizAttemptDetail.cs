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
    
    public partial class LMSStudentQuizAttemptDetail
    {
        public int LMSStudentQuizAttemptDetailID { get; set; }
        public Nullable<int> LMSStudentQuizAttemptID { get; set; }
        public int LMSQuizIQuestionD { get; set; }
        public int LMSQuizID { get; set; }
        public Nullable<int> LMSQuestionTypeID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<bool> IsNegativeMarking { get; set; }
        public Nullable<bool> AnswerMCQ1 { get; set; }
        public Nullable<bool> AnswerMCQ2 { get; set; }
        public Nullable<bool> AnswerMCQ3 { get; set; }
        public Nullable<bool> AnswerMCQ4 { get; set; }
        public Nullable<bool> AnswerTrueFalse { get; set; }
        public Nullable<bool> AnswerSCQ1 { get; set; }
        public Nullable<bool> AnswerSCQ2 { get; set; }
        public Nullable<bool> AnswerSCQ3 { get; set; }
        public Nullable<bool> AnswerSCQ4 { get; set; }
        public Nullable<bool> OverallAnswer { get; set; }
        public Nullable<decimal> QuestionMarks { get; set; }
        public Nullable<decimal> ObtainMarks { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }

    public class LMSStudentQuizAttemptDetailViewModel
    {
        public List<LMSStudentQuizAttemptDetail> LMSStudentQuizAttemptDetails { get; set; }
        public LMSStudentQuizAttemptDetail SelectedLMSStudentQuizAttemptDetails { get; set; }
        public string DisplayMode { get; set; }
    }
}