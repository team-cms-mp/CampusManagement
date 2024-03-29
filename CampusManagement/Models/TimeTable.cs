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
    using System.ComponentModel.DataAnnotations;

    public partial class TimeTable
    {
        [Display(Name = "TimeTableID")]
        public int TimeTableID { get; set; }
        [Display(Name = "Time Slot ID")]
        public int TimeSlotID { get; set; }
        [Display(Name = "Room")]
        public string RoomName { get; set; }
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Display(Name = "Teacher")]
        public string TeacherName { get; set; }
        [Display(Name = "Day")]
        public string DayName { get; set; }
        [Display(Name = "Batch")]
        public string BatchName { get; set; }
        [Display(Name = "Semester")]
        public string SemesterName { get; set; }
        [Display(Name = "Program")]
        public string ProgramName { get; set; }
        [Display(Name = "Time Slot")]
        public string TimeSlot { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        [Display(Name = "Created On")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Display(Name = "Created By")]
        public Nullable<int> CreatedBy { get; set; }
        [Display(Name = "Is Active")]
        public string IsActive { get; set; }
        [Display(Name = "Modified On")]
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [Display(Name = "Modified By")]
        public Nullable<int> ModifiedBy { get; set; }
    }
}
