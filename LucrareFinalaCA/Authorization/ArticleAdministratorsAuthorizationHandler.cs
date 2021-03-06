﻿using LucrareFinalaCA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Authorization
{
    public class ArticleAdministratorsAuthorizationHandler:
                    AuthorizationHandler<OperationAuthorizationRequirement,Article>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Article resource)
        {
            if(context.User ==null)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.ArticleAdministratorsRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
