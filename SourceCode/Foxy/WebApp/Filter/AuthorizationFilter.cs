using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            //if (context.ActionDescriptor.DisplayName == "Profile")
            //{
      
                var x = 6;
            //}
        }
    }
}
