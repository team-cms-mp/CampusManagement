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
    
    public partial class ExamDateAvailableSolt
    {
        public int ExamDateAvailableSoltID { get; set; }
        public Nullable<int> ExamID { get; set; }
        public Nullable<int> ExamDateID { get; set; }
        public Nullable<int> ExamDateTimeSlotID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}