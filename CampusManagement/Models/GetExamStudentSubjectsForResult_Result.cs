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
    public partial class GetExamStudentSubjectsForResult_Result
    {
        public int ExamsDateSheetDetailID { get; set; }
        public Nullable<int> ExamID { get; set; }
        public Nullable<int> ExamDateID { get; set; }
        public Nullable<int> ExamDateTimeSlotID { get; set; }
        public Nullable<int> ProgramCourseID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> StudentBatchProgramCourseID { get; set; }
        public Nullable<int> BatchProgramID { get; set; }
        public Nullable<int> CourseID { get; set; }
        public Nullable<int> CourseTypeID { get; set; }
        public Nullable<int> YearSemesterNo { get; set; }
        public string BatchName { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public string ExamSubjectTitle { get; set; }
    }

    public class ExamStudentSubjectsForResult {
        public List<GetExamStudentSubjectsForResult_Result> SubjectList { get; set; }
        public Nullable<int> ExamID { get; set; }
        public Nullable<int> StudentID { get; set; }

    }
}
