using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Authorization;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LucrareFinalaCA
{
    [AllowAnonymous]
    public class ArticleModel : PageModel
    {
        private readonly ArticleController _articleController;
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

        public ArticleModel(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _articleController = new ArticleController(ctx, authorizationService,userManager);
        }

        public async Task<IActionResult> OnGet(int? qid = null)
        {
            if (qid != null)
                return await OnGetWithId(qid.Value);
            Articles = await _articleController.GetAsync();
            return Page();
        }
        public async Task<IActionResult> OnGetWithId(int id)
        {
            Article = await _articleController.GetByIdAsync(id);
            IsbyId = true;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            //CRUD will be modified to take a user view model as a parameter and work only for the appropriate users.
            await _articleController.Delete(id, User);
            return RedirectToPage("./Index");
        }
    }
}