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

    public partial class Designation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Designation()
        {
            this.Teachers = new HashSet<Teacher>();
        }

        [Display(Name = "Designation ID")]
        public int DesignationID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teacher> Teachers { get; set; }
    }

    public class DesignationsViewModel
    {
        public List<Designation> Designations { get; set; }
        public Designation SelectedDesignation { get; set; }
        public string DisplayMode { get; set; }
    }
}
