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
    public partial class StudentProgramEnrollment
    {

        [Display(Name = "Enrollment ID")]
        public int EnrollmentID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Student")]
        public int StudentID { get; set; }
        [Display(Name = "Program")]
        public int BatchProgramID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Admission Date")]
        public string AdmissionDate { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Enrollment #")]
        public string EnrollmentNo { get; set; }
        [Display(Name = "Registration #")]
        public string RegistrationNo { get; set; }
        [Display(Name = "Degree Completed")]
        public string DegreeCompleted { get; set; }
        [Display(Name = "Degree Completion Date")]
        public string DegreeCompletionDate { get; set; }
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

        public virtual BatchProgram BatchProgram { get; set; }
        public virtual Student Student { get; set; }
    }

    public class StudentProgramEnrollmentsViewModel
    {
        public List<StudentProgramEnrollment> StudentProgramEnrollments { get; set; }
        public StudentProgramEnrollment SelectedStudentProgramEnrollment { get; set; }
        public string DisplayMode { get; set; }
    }
}
