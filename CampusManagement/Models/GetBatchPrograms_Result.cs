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

    public partial class GetBatchPrograms_Result
    {

        public int BatchProgramID { get; set; }
        [Display(Name = "Session")]
        public int BatchID { get; set; }
        [Display(Name = "Session")]
        public string BatchName { get; set; }
        [Display(Name = "Program")]
        public int ProgramID { get; set; }
        [Display(Name = "Program")]
        public string ProgramName { get; set; }
        [Display(Name = "Created On")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Display(Name = "Created By")]
        public Nullable<int> CreatedBy { get; set; }
        [Display(Name = "Is Active")]
        public string IsActive { get; set; }
        [Display(Name = "Modified By")]
        public Nullable<int> ModifiedBy { get; set; }
        [Display(Name = "Modified On")]
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [Display(Name = "PerCredit Hour Fee")]
        public string PerCreditHourFee { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Admission Date From")]
        public string AdmissionDateFrom { get; set; }
        [Display(Name = "Admission Date To")]
        public string AdmissionDateTo { get; set; }
        [Display(Name = "Registration Date From")]
        public string RegistrationDateFrom { get; set; }
        [Display(Name = "Registration Date To")]
        public string RegistrationDateTo { get; set; }
        [Display(Name = "Orientation Date To")]
        public string OrientationDateTo { get; set; }
        [Display(Name = "Orientation Date From")]
        public string OrientationDateFrom { get; set; }
        [Display(Name = "Selection Percentage")]
        public string SelectionPercentage { get; set; }
        [Display(Name = "Entry Test Percentage")]
        public string EntryTestPercentage { get; set; }
        [Display(Name = "Entry Test Result Date")]
        public string EntryTestResultDate { get; set; }
        [Display(Name = "Entry Test 1 Date")]
        public string EntryTestDate { get; set; }
        [Display(Name = "Entry Test 2 Date")]
        public string EntryTestDate1 { get; set; }
        [Display(Name = "Entry Test 3 Date")]
        public string EntryTestDate2 { get; set; }
        [Display(Name = "Entry Test 4 Date")]
        public string EntryTestDate3 { get; set; }
        [Display(Name = "First Merit List Date")]
        public string FirstMeritListDate { get; set; }
        [Display(Name = "Second Merit List Date")]
        public string SecondMeritListDate { get; set; }
        [Display(Name = "Third Merit List Date")]
        public string ThirdMeritListDate { get; set; }
        [Display(Name = "Fourth Merit List Date")]
        public string FourthMeritListDate { get; set; }
        [Display(Name = "Sending Offer Letter Date")]
        public string SendingOfferLetterDate { get; set; }
        [Display(Name = "Add Drop Dead line Date From")]
        public string AddDropDeadlineDateFrom { get; set; }
        [Display(Name = "Add Drop Dead line Date To")]
        public string AddDropDeadlineDateTo { get; set; }
        [Display(Name = "Withdrawal Subject Date From")]

        public string WithdrawalSubjectDateFrom { get; set; }
        [Display(Name = "Withdrawal Subject Date To")]
        public string WithdrawalSubjectDateTo { get; set; }
        [Display(Name = "Withdrawal Subject Percentage")]
        public string WithdrawalSubjectPercentage { get; set; }
        [Display(Name = "GAT NAT Percentage")]
        public string GATNATPercentage { get; set; }
        [Display(Name = "Current Semester")]
        public Nullable<int> CurrentSemesterNo { get; set; }
        [Display(Name = "Shift")]
        public Nullable<int> ShiftID { get; set; }
        [Display(Name = "Shift Name")]
        public string ShiftName { get; set; }

    }
    public class GetBatchPrograms_ResultViewModel
    {
        public List<GetBatchPrograms_Result> GetBatchPrograms_Results { get; set; }
        public GetBatchPrograms_Result SelectedGetBatchPrograms_Result { get; set; }
        public string DisplayMode { get; set; }
    }
}