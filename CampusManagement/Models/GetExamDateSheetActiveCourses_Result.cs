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
    
    public partial class GetExamDateSheetActiveCourses_Result
    {
        public int ProgramCourseID { get; set; }
        public int BatchProgramID { get; set; }
        public int CourseID { get; set; }
        public Nullable<int> CourseTypeID { get; set; }
        public int YearSemesterNo { get; set; }
        public string BatchName { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public string ExamSubjectTitle { get; set; }
    }
}
