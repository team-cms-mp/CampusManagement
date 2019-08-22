using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusManagement.App_Code
{
    public class Enums
    {
        enum Status
        {
            Selected = 7,
            Pending = 1007,
            Closed = 1020,
            Pending_Further_Investigation = 1021,
            Under_Investigation = 1022,
            Not_Eligible = 1023,
            Not_Submitted = 1024,
            Submitted = 1025,
            In_Progress = 1026,
            Completed = 1027,
            Deferred = 1028,
            Not_Selected = 1029,
            Show_Admit_Card = 2008,
            Fail = 2009,
            Pass = 2010,
            Promoted = 2011,
            Approved = 3009,
            Unapproved = 3010,
            Blocked = 3011,
            Subjects_Approval = 4011,
            Subjects_Approved = 4012,
            Challan_Generated = 4013,
            Entry_Test_Email_Sent = 4014,
            Offer_Letter_Email_Sent = 4015,
            Orientation_Email_Sent = 5014,
            Unapprove_Student = 5015,
            Approve_Student = 6015,
        }

        enum MaritalStatus
        {
            Married = 1,
            Single = 2,
        }

        enum Salutation
        {
            Mr = 1,
            Mrs = 2,
            Miss = 4,
        }

        enum CollegeService
        {
            Admission_Fee = 1,
            Tuition_Fee = 2,
            Library_Fee = 4,
            Student_Fund = 1002,
            Late_Fee_Fine = 3002,
            Short_Attendance_Fine = 3003,
            Rule_Violation_Fine = 3004,
        }

        enum CourseType
        {
            Compulsory = 2,
            Elective = 3,
        }

        enum Day
        {
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6,
            Sunday = 7,
        }

        enum Degree
        {
            SSC_OLevel = 20,
            Interview = 21,
            Test = 22,
            HSSC_ALevel = 1020,
            Bachelors = 1021,
            Masters = 1022,
            MS_MPhil = 1023,
            NAT = 2020,
            GAT = 2021,
            SAT = 2022,
            GRE = 2023,
            GMAT = 2024,
            Drawing_Test = 4020,
        }

        enum DegreeTitle
        {
            Computer_Science = 1,
            ICom = 2,
            ICS = 3,
            BSCS = 4,
            BCom = 5,
            MSCS = 6,
            MS_MPhil = 7,
            Others = 8,
            text = 9,
        }

        enum DepositType
        {
            Cash = 1,
            Bank = 3,
            Online = 4,
        }

        enum FormType
        {
            Prospectus = 1,
        }

        enum Gender
        {
            Male = 1,
            Female = 2,
        }

        enum Level
        {
            Diploma_12_Years_Education = 1,
            Diploma_14_Years_Education = 2,
            Diploma_16_Years_Education = 3,
            Diploma_18_Years_Education = 4,
        }

        enum LMSQuestionType
        {
            MCQ = 1,
            SCQ = 2,
            True_False = 3,
        }

        enum LMSWeekListType
        {
            Lecture = 2003,
            Assignment = 2004,
            Quiz = 2005,
        }
    }
}