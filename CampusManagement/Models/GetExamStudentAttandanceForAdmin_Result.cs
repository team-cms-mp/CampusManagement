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
    
    public partial class GetExamStudentAttandanceForAdmin_Result
    {
        public int ExamEligibleStudentsID { get; set; }
        public Nullable<int> SeatNumber { get; set; }
        public int StudentPresentStatusID { get; set; }
        public string Remarks { get; set; }
        public string StudentPresentStatusName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string BatchSession { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public int YearSemesterNo { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDateTitle { get; set; }
        public string TimeSlot { get; set; }
        public Nullable<int> StudentID { get; set; }
        public string FormNo { get; set; }
        public Nullable<int> AddAppID { get; set; }
        public string studentname { get; set; }
        public string CellNo { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string RollNumber { get; set; }
        public string RegistrationNo { get; set; }
    }

    public class ExamStudentAttandanceForAdminViewModel
    {

        public int ExamID { get; set; }
        public int ExamDateID { get; set; }
        public int ExamDateTimeSlotID { get; set; }
        public int BatchID { get; set; }
        public int BatchProgramID { get; set; }
        public int YearSemesterNo { get; set; }
        public int ProgramCourseID { get; set; }
        public List<GetExamStudentAttandanceForAdmin_Result> StudentList { get; set; }

    }
}
