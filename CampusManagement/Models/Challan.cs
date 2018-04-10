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
    public partial class Challan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Challan()
        {
            this.ChallanServices = new HashSet<ChallanService>();
            this.Deposits = new HashSet<Deposit>();
        }

        public int ChallanID { get; set; }
        [Display(Name = "Form #")]
        public string FormNo { get; set; }
        [Display(Name = "Issue Date")]
        public Nullable<System.DateTime> IssueDate { get; set; }
        [Display(Name = "Last Date")]
        public Nullable<System.DateTime> LastDate { get; set; }
        [Display(Name = "Bank Account")]
        public Nullable<int> AccountID { get; set; }
        [Display(Name = "Is Deposited")]
        public string IsDeposited { get; set; }
        [Display(Name = "Year/Semester #")]
        public Nullable<int> YearSemesterNo { get; set; }
        public Nullable<int> Voucher_Trans_ID { get; set; }
        [Display(Name = "Created By")]
        public Nullable<int> CreatedBy { get; set; }
        [Display(Name = "Created On")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Display(Name = "Is Active")]
        public string IsActive { get; set; }
        [Display(Name = "Modified On")]
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [Display(Name = "Modified By")]
        public Nullable<int> ModifiedBy { get; set; }
        [Display(Name = "Student ID")]
        public Nullable<int> StudentID { get; set; }
        [Display(Name = "Program")]
        public Nullable<int> BatchProgramID { get; set; }

        // Custom code added
        public Nullable<int> Quantity { get; set; }

        public virtual BatchProgram BatchProgram { get; set; }
        public virtual Student Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChallanService> ChallanServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deposit> Deposits { get; set; }
    }

    public class ChallansViewModel
    {
        public List<GetAllChallans_Result> Challans { get; set; }
        public GetAllChallans_Result SelectedChallan { get; set; }
        public string DisplayMode { get; set; }
    }
}