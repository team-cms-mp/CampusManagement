using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampusManagement.Models;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.IO;

namespace CampusManagement.App_Code
{
    public class CommonFunctions
    {
        private static ModelCMSNewContainer db = new ModelCMSNewContainer();
        public static string GetRandomString(int seed)
        {
            // Random is not truly random,
            // so we try to encourage better randomness by always changing the seed value
            Random rnd = new Random((seed + DateTime.Now.Millisecond));

            // basic 5 digit random number
            string result = rnd.Next(10000, 99999).ToString();

            // random position to put the alpha character
            int replacementIndex = rnd.Next(0, (result.Length - 1));

            return result;
        }

        public static List<GetDepartments_by_HospitalID_Result> GetDepartments(int HospitalID)
        {
            try
            {
                List<GetDepartments_by_HospitalID_Result> lstDepartments = new List<GetDepartments_by_HospitalID_Result>();
                lstDepartments = db.GetDepartments_by_HospitalID(HospitalID).ToList();
                return lstDepartments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GetSubDepartments_by_HospitalID_Result> GetSubDepartments(int HospitalID)
        {
            try
            {
                List<GetSubDepartments_by_HospitalID_Result> lstSubDepartments = new List<GetSubDepartments_by_HospitalID_Result>();
                lstSubDepartments = db.GetSubDepartments_by_HospitalID(HospitalID).ToList();
                return lstSubDepartments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetSubDepartmentNameByDepartmentID(int HospitalID, int SubDepartmentID)
        {
            try
            {
                string SubDeptName = "";
                GetSubDepartments_by_HospitalID_Result objSubDept = new GetSubDepartments_by_HospitalID_Result();
                objSubDept = db.GetSubDepartments_by_HospitalID(HospitalID).FirstOrDefault();
                if (objSubDept != null)
                {
                    SubDeptName = objSubDept.SubDept_Name;
                }
                return SubDeptName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GEt programs by Faculty and Batch.
        public static List<GetApplicantStudentStatus_Result> GetApplicantStudentStatus(string StatusType, int? QueryID)
        {
            List<GetApplicantStudentStatus_Result> lstStatus = new List<GetApplicantStudentStatus_Result>();

            lstStatus = db.GetApplicantStudentStatus(StatusType, QueryID).ToList();
            var status = lstStatus.Select(s => new
            {
                StatusID = s.StatusID,
                StatusName = s.StatusName,
                StatusType = s.StatusType
            });
            return lstStatus;
        }
    }
}