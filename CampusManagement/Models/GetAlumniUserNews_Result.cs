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

    public partial class GetAlumniUserNews_Result
    {
        public int AlumniUserNewsID { get; set; }
        public Nullable<int> AlumniUserID { get; set; }
        public string NewsContent { get; set; }
        public string NewsTitle { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string UserNameCMS { get; set; }
        public string EmailCMS { get; set; }
        public byte[] PictureCMS { get; set; }
    }

    public class GetAlumniUserNews_ResultViewModel
    {
        public List<GetAlumniUserNews_Result> GetAlumniUserNews_Results { get; set; }
        public GetAlumniUserNews_Result SelectedGetAlumniUserNews_Results { get; set; }
        public string DisplayMode { get; set; }
    }
}