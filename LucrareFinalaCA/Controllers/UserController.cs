using LucrareFinalaCA.Authorization;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Controllers
{
    public class UserController
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly ApplicationDbContext _ctx;
        protected readonly IAuthorizationService _authorizationService;
        protected readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext ctx, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _ctx = ctx;
            _roleManager = roleManager;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var rv = new List<UserViewModel>();
            var users = await _ctx.Users.ToListAsync();
            foreach (var user in users)
            {
                var vm = new UserViewModel()
                {
                    UserName = user.UserName
                };
                rv.Add(vm);
            }
            return rv;
        }
        public async Task<IdentityResult> AssignRoleAsync(string userName, string role)
        {
            IdentityResult IR = null;
            IdentityUser User = await _userManager.FindByNameAsync(userName);
            var uId = User.Id;

            if (_roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                IR = await _roleManager.CreateAsync(new IdentityRole(role));
            }


            var user = await _userManager.FindByIdAsync(uId);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await _userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
