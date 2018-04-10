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

    public partial class Department
    {
        [Display(Name = "Department ID")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public string Description { get; set; }
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

    public class DepartmentsViewModel
    {
        public List<Department> Departments { get; set; }
        public Department SelectedDepartment { get; set; }
        public string DisplayMode { get; set; }
    }
}