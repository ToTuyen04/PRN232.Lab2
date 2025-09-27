using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class AdminRoleRequirement : IAuthorizationRequirement { }
    
    public class AdminRoleHandler : AuthorizationHandler<AdminRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleRequirement requirement)
        {
            if (!context.User.IsInRole("Admin"))
            {
                throw new ForbiddenException("You do not have permission to access this function.");
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
