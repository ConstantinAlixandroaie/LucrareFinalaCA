using LucrareFinalaCA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Authorization
{
    public class ArticleManagerAuthorizationHandler :
         AuthorizationHandler<OperationAuthorizationRequirement, Article>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Article resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }
            //can do any operation on any article besides delete.
            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName)
            {
                return Task.CompletedTask;
            }
            if (context.User.IsInRole(Constants.ArticleManagersRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
