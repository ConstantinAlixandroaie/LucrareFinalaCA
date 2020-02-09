using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LucrareFinalaCA.Authorization;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LucrareFinalaCA
{
    [AllowAnonymous]
    public class ArticleModel : PageModel
    {
        private readonly ArticleController _articleController;
        private readonly CategoryController _categoryController;
        [BindProperty]
        public List<ArticleViewModel> Articles { get; set; }
        [BindProperty]
        public ArticleViewModel Article { get; set; }
        [BindProperty]
        protected IAuthorizationService AuthorizationService { get; }
        [BindProperty]
        public bool IsbyId { get; set; }
        [BindProperty]
        public string UserID { get; set; }
        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }
        [BindProperty]
        public List<CategoryViewModel> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }
         
        public ArticleModel(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _articleController = new ArticleController(ctx, authorizationService, userManager);
            _categoryController = new CategoryController(ctx, authorizationService, userManager);
        }

        
        public async Task<IActionResult> OnGet(int? qid = null)
        {
            Categories = await _categoryController.GetAsync();
            if (qid != null)
                return await OnGetWithId(qid.Value, User);

            if (!string.IsNullOrEmpty(searchString)||!string.IsNullOrEmpty(Category))
            {
                Articles = await _articleController.GetByLastAsync(searchString,Category);
            }
            else
                Articles = await _articleController.GetAsync();
            return Page();
        }
        public async Task<IActionResult> OnGetWithId(int id, ClaimsPrincipal user)
        {
            Article = await _articleController.GetByIdAsync(id, user);
            IsbyId = true;
            return Page();
        }
        [HttpPost]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            //CRUD will be modified to take a user view model as a parameter and work only for the appropriate users.
            await _articleController.Delete(id, User);
            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostApprove(int id)
        {
            await _articleController.ApproveArticle(id, User);
            return Page();

        }
    }
}