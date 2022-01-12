using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Alerting.Presentation.Attributes
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public AuthAttribute(params string[] roles)
        {
            if (roles.Any())
            {
                Roles = $"{string.Join(", ", roles)}, root";
            }
        }
    }
}