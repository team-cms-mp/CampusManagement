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

    public partial class RelationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RelationType()
        {
            this.Applicants = new HashSet<Applicant>();
            this.Students = new HashSet<Student>();
        }

        [Display(Name = "Relation Type ID")]
        public int RelationTypeID { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Relation Type")]
        public string RelationTypeName { get; set; }
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
        public virtual ICollection<Applicant> Applicants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
    }

    public class RelationTypesViewModel
    {
        public List<RelationType> RelationTypes { get; set; }
        public RelationType SelectedRelationType { get; set; }
        public string DisplayMode { get; set; }
    }
}
