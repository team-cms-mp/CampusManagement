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

    public partial class GetChatUser_Result
    {
        public int EmpID { get; set; }
        public string UserName { get; set; }
        public string Designation { get; set; }
    }

    public class ChatUserViewModel
    {

        public List<GetChatUser_Result> AdminList { get; set; }
        public List<GetChatUser_Result> LecturarList { get; set; }
        public List<GetChatUser_Result> StudentList { get; set; }
    }
}