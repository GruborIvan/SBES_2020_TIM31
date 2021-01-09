using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomPrincipal : IPrincipal
    {
        WindowsIdentity identity = null;
        public IIdentity Identity
        {
            get { return identity; }
        }

        public CustomPrincipal(WindowsIdentity windowsIdentity)
        {
            identity = windowsIdentity;
        }

        public bool IsInRole(string permission)
        {
            Console.WriteLine($"Checking permission {permission}");
            foreach (IdentityReference group in this.identity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));

                //Console.WriteLine($"Full name: {name.ToString()}");
                string groupName = Formatter.ParseName(name.ToString());
                //Console.WriteLine($"Formatted name: {groupName.ToString()}");
                string[] permissions;

                if (RolesConfig.GetPermissions(groupName, out permissions))
                {
                    if (permissions.Contains(permission))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                Console.WriteLine($"Group {groupName} doesn't have permission {permission}");
            }
            return false;
        }
    }
}
