using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mindscape.LightSpeed;

namespace FairlieAuthentic.Models
{
    public class FARoleProvider : RoleProvider
    {
        private static LightSpeedContext<FairlieAuthenticUnitOfWork> _context;
        public override string ApplicationName { get; set; }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            _context = new LightSpeedContext<FairlieAuthenticUnitOfWork>("FA");
            using (var uow = _context.CreateUnitOfWork())
            {
                var user = uow.Customers.FirstOrDefault(x => x.Name == username);
                if (user == null)
                {
                    return null;
                }
                else
                {
                    string[] roles = user.Roles.Select(x => x.RoleName).ToArray();
                    return roles;
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
    }
}