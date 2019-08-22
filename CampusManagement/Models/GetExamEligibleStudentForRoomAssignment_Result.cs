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

    public partial class GetExamEligibleStudentForRoomAssignment_Result
    {
        public Nullable<int> StudentCount { get; set; }
        public Nullable<int> ExamsDateSheetDetailID { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string BatchSession { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public Nullable<int> YearSemesterNo { get; set; }
    }


    public class ExamEligibleStudentForRoomAssignmentViewModel {

        public List<GetExamEligibleStudentForRoomAssignment_Result> CourseList { get; set; }
        public List<GetExamRoomsForRoomAssignment_Result> AvailabeRoomList { get; set; }
        public int ExamID { get; set; }
        public int ExamDateID { get; set; }
        public int ExamDateTimeSlotID { get; set; }
       


    }

    public class ExamEligibleStudentForRoomAssignmentTestViewModel
    {

        public List<GetExamEligibleStudentForRoomAssignment_Result> CourseList { get; set; }
        public int ExamID { get; set; }
        public int ExamDateID { get; set; }
        public int ExamDateTimeSlotID { get; set; }



    }
}