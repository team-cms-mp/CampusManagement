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
    
    public partial class StudentBatchProgramCourse
    {
        public int StudentBatchProgramCourseID { get; set; }
        public Nullable<int> ProgramCourseID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> CourseID { get; set; }
        public Nullable<int> YearSemesterNo { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> BatchProgramID { get; set; }
    
        public virtual BatchProgramCourse BatchProgramCourse { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }

    public class StudentBatchProgramCoursesViewModel
    {
        public List<StudentBatchProgramCourse> StudentBatchProgramCourses { get; set; }
        public StudentBatchProgramCourse SelectedStudentBatchProgramCourse { get; set; }
        public string DisplayMode { get; set; }
    }
}
