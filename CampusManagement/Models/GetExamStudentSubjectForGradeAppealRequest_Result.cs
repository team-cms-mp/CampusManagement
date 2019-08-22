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
    using System.Web.Mvc;

    public partial class GetExamStudentSubjectForGradeAppealRequest_Result
    {
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> ExamID { get; set; }
        public Nullable<int> ProgramCourseID { get; set; }
        public int CourseID { get; set; }
        public string ExamSubjectTitle { get; set; }
        public string BatchName { get; set; }
        public string BatchSession { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public Nullable<int> YearSemesterNo { get; set; }
        public bool IsGradeAppealRequest { get; set; }
    }

    public class ExamStudentSubjectForGradeAppealRequestViewModel {
        public int ExamID { get; set; }
        //public  SelectList TeacherListSelected  { get; set; }
        public List<GetExamStudentSubjectForGradeAppealRequest_Result> SubjectList { get; set; }
        public List<GetExamGradeAppealStudentCourses_Result> SubjectListAppeal { get; set; }
        public GetExamGradeAppealFormForStudent_Result ExamGradeAppealFormForStudentObj { get; set; }

        public GetStudentDetailByStudentID_Result StudentDetailObj { get; set; }
    }
}