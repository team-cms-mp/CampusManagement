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

    public partial class GetAlumniUserBlog_Result
    {
        public int AlumniUserBlogID { get; set; }
        public Nullable<int> AlumniUserID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
    }

    public class GetAlumniUserBlog_ResultViewModel
    {
        public List<GetAlumniUserBlog_Result> GetAlumniUserBlog_Results { get; set; }
        public GetAlumniUserBlog_Result SelectedGetAlumniUserBlog_Results { get; set; }
        public string DisplayMode { get; set; }
    }
}
