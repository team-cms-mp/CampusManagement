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

    public partial class GetAlumniUserEvent_Result
    {
        public int AlumniUserEventID { get; set; }
        public string EventTitle { get; set; }
        public string EventDetails { get; set; }
        public string EventLocation { get; set; }
        public Nullable<System.DateTime> EventStartDateTime { get; set; }
        public Nullable<System.DateTime> EventEndDateTime { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UserName { get; set; }
    }

    public class GetAlumniUserEvent_ResultViewModel
    {
        public List<GetAlumniUserEvent_Result> GetAlumniUserEvent_Results { get; set; }
        public GetAlumniUserEvent_Result SelectedGetAlumniUserEvent_Results { get; set; }
        public string DisplayMode { get; set; }
    }
}
