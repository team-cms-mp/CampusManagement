using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CampusManagement.Models;

namespace CampusManagement.MyRoleProvider
{
    public class SiteRole : RoleProvider
    {
        ModelUserManagementContainer db = new ModelUserManagementContainer();
        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string roleData = string.Empty;
            var dn = db.GetLoginUser(username, null).FirstOrDefault();
            if (dn == null)
            {
                roleData = string.Empty;
            }
            else
            {
                roleData = db.GetLoginUser(username, null).FirstOrDefault().Designation_Name;
            }

            if(string.IsNullOrEmpty(roleData))
            {
                roleData = string.Empty;
            }

            string[] results = { roleData };
            return results;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}