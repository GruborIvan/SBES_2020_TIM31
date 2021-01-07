using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Provera da li postoji permisija za Read. S obzirom da svi treba da imaju permisiju da citaju, zabrana pristupa 
            CustomPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as CustomPrincipal;

            return principal.IsInRole("Read");
        }
    }
}
