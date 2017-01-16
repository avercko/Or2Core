﻿using System.Collections.Generic;
using Orchard.Security.Permissions;

namespace Orchard.OpenId
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageOpenIdApplications = new Permission("ManageOpenIdApplications", "Managing Open Id Applications");
        
        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                ManageOpenIdApplications
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageOpenIdApplications }
                }
            };
        }

    }
}
