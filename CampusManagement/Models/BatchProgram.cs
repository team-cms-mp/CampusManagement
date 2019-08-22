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

    public partial class BatchProgram
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BatchProgram()
        {
            this.Applicants = new HashSet<Applicant>();
            this.BatchProgramCourses = new HashSet<BatchProgramCourse>();
            this.BatchProgramSemesters = new HashSet<BatchProgramSemester>();
            this.Challans = new HashSet<Challan>();
            this.FormSaleDetails = new HashSet<FormSaleDetail>();
            this.SelectionCriterias = new HashSet<SelectionCriteria>();
            this.Students = new HashSet<Student>();
            this.StudentProgramEnrollments = new HashSet<StudentProgramEnrollment>();
        }

        public int BatchProgramID { get; set; }
        [Display(Name = "Session")]
        public int BatchID { get; set; }
        [Display(Name = "Program")]
        public int ProgramID { get; set; }
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
        [Display(Name = "Per Credir Hour Fee")]
        public string PerCreditHourFee { get; set; }
        [Display(Name = "Program Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "Program End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Admission Date From")]
        public string AdmissionDateFrom { get; set; }
        [Display(Name = "Admission Date To")]
        public string AdmissionDateTo { get; set; }
        [Display(Name = "Registration Date From")]
        public string RegistrationDateFrom { get; set; }
        [Display(Name = "Registration Date To")]
        public string RegistrationDateTo { get; set; }
        [Display(Name = "Orientation Date From")]
        public string OrientationDateFrom { get; set; }
        [Display(Name = "Orientation Date To")]
        public string OrientationDateTo { get; set; }
        [Display(Name = "Selection Percentage")]
        public string SelectionPercentage { get; set; }
        [Display(Name = "Entry Test Date")]
        public string EntryTestDate { get; set; }
        [Display(Name = "Entry Test Result Date 1")]

        public string EntryTestDate1 { get; set; }
        [Display(Name = "Entry Test Result Date 2")]

        public string EntryTestDate2 { get; set; }
        [Display(Name = "Entry Test Result Date 3")]

        public string EntryTestDate3 { get; set; }
        [Display(Name = "Entry Test Result Date 4")]
        public string EntryTestResultDate { get; set; }
        [Display(Name = "First Merit List Date")]
        public string FirstMeritListDate { get; set; }
        [Display(Name = "Second Merit List Date")]
        public string SecondMeritListDate { get; set; }
        [Display(Name = "Third Merit List Date")]
        public string ThirdMeritListDate { get; set; }
        [Display(Name = "Fourth Merit List Date")]
        public string FourthMeritListDate { get; set; }
        [Display(Name = "Sendting Offer Letter Date")]
        public string SendingOfferLetterDate { get; set; }
        [Display(Name = "Add/Drop Date From")]
        public string AddDropDeadlineDateFrom { get; set; }
        [Display(Name = "Add/Drop Date To")]
        public string AddDropDeadlineDateTo { get; set; }
        [Display(Name = "Withdrawal Subject Date From")]
        public string WithdrawalSubjectDateFrom { get; set; }
        [Display(Name = "Withdrawal Subject Date To")]
        public string WithdrawalSubjectDateTo { get; set; }
        [Display(Name = "Withdrawal Percentage")]
        public string WithdrawalSubjectPercentage { get; set; }
        [Display(Name = "Entry Test Percentage")]
        public string EntryTestPercentage { get; set; }
        [Display(Name = "GAT/NAT Percentage")]
        public string GATNATPercentage { get; set; }
        public Nullable<int> CurrentSemesterNo { get; set; }
        public Nullable<int> ShiftID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Applicant> Applicants { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Program Program { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchProgramCourse> BatchProgramCourses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchProgramSemester> BatchProgramSemesters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Challan> Challans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormSaleDetail> FormSaleDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SelectionCriteria> SelectionCriterias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentProgramEnrollment> StudentProgramEnrollments { get; set; }
    }

    public class BatchProgramViewModel
    {
        public List<BatchProgram> BatchPrograms { get; set; }
        public BatchProgram SelectedBatchProgram { get; set; }
        public string DisplayMode { get; set; }
    }
}
