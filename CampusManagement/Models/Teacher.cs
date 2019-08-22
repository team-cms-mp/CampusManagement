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

    public partial class Teacher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Teacher()
        {
            this.TeachersCourseAllocations = new HashSet<TeachersCourseAllocation>();
        }

        [Display(Name = "Teacher ID")]
        public int TeacherID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Mobile #")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "CNIC")]
        public string CNIC { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Qualification { get; set; }
        [Display(Name = "Designation")]
        public int DesignationID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Employee #")]
        public string EmployeeNo { get; set; }
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
    
        public virtual Designation Designation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeachersCourseAllocation> TeachersCourseAllocations { get; set; }
    }

    public class TeachersViewModel
    {
        public List<Teacher> Teachers { get; set; }
        public Teacher SelectedTeacher { get; set; }
        public string DisplayMode { get; set; }
    }
}
