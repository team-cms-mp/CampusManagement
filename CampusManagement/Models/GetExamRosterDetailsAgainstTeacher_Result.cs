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

    public partial class GetExamRosterDetailsAgainstTeacher_Result
    {
        public string ExamTitle { get; set; }
        public string SeasonName { get; set; }
        public string TermName { get; set; }
        public string ExamDateTitle { get; set; }
        public string TimeSlot { get; set; }
        public string RoomName { get; set; }
        public string Designation_Name { get; set; }
        public string EFName { get; set; }
        public string ELName { get; set; }
        public string CNIC { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string BatchCode { get; set; }
        public string BatchSession { get; set; }
    }

    public class ExamRosterDetailsTeacherViewModel {
        public int ExamID { get; set; }
        public int EmpID { get; set; }
        public List<GetExamRosterDetailsAgainstTeacher_Result> RosterDetailList { get; set; }
       
    }

}
