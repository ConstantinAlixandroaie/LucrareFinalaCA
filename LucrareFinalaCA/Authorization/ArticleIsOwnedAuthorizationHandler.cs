﻿using LucrareFinalaCA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Authorization
{
    public class ArticleIsOwnedAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Article>
    {
        UserManager<IdentityUser> _userManager;

        public ArticleIsOwnedAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                         OperationAuthorizationRequirement requirement,
                                                         Article resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName)
                //&& requirement.Name != Constants.DeleteOperationName)  
                //article author is only able to create update or read but not able to remove. only the administrator can do it. 
            {
                return Task.CompletedTask;
            }

            if (resource.Author == _userManager.GetUserName(context.User))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

    }
}
