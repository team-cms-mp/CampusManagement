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
    
    public partial class GetAllOBE_ProgramPLO_Result
    {
        public int PLOID { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public string PLOCode { get; set; }
        public string PLOName { get; set; }
        public Nullable<int> PLOTypeID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
    }
}