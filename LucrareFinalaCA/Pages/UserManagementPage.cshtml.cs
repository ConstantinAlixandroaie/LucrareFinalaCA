using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LucrareFinalaCA.Authorization;
using Microsoft.AspNetCore.Identity;
using LucrareFinalaCA.Data;
using Microsoft.AspNetCore.Authorization;

namespace LucrareFinalaCA
{

    [Authorize(Roles = "ArticlesAdministrators")]
    public class UserManagementPageModel : PageModel
    {
        private readonly UserController _userController;
        public  List<string> userRoles;
        [BindProperty]
        public List<UserViewModel> Users { get; set; }
        [BindProperty]
        public string selectedUser { get; set; }
        [BindProperty]
        public string selectedRole { get; set; }
        
        public UserManagementPageModel(UserManager<IdentityUser> userManager, ApplicationDbContext ctx, RoleManager<IdentityRole> roleManager,IAuthorizationService authorizationService)
        {
            _userController = new UserController(userManager, ctx, roleManager,authorizationService);
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            userRoles = new List<string>();
            userRoles.Add(Constants.ArticleAdministratorsRole);
            userRoles.Add(Constants.ArticleManagersRole);
            userRoles.Add(Constants.ArticleEditorRole);
            Users = await _userController.GetUsersAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostPromote()
        {
            await _userController.AssignRoleAsync(selectedUser, selectedRole,User);
            return RedirectToPage("/UserManagementPage");
        }
    }
}