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

    public partial class Semester
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Semester()
        {
            this.BatchProgramSemesters = new HashSet<BatchProgramSemester>();
            this.Challans = new HashSet<Challan>();
        }

        [Display(Name = "Semester ID")]
        public int SemesterID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Semester #")]
        public int YearSemesterNo { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Semester Name")]
        public string SemesterName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Semester Code")]
        public string SemesterCode { get; set; }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchProgramSemester> BatchProgramSemesters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Challan> Challans { get; set; }
    }

    public class SemestersViewModel
    {
        public List<Semester> Semesters { get; set; }
        public Semester SelectedSemester { get; set; }
        public string DisplayMode { get; set; }
    }
}