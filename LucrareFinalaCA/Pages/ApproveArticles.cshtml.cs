using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LucrareFinalaCA
{
    [Authorize(Roles = "ArticlesAdministrators")]
    public class ApproveArticlesModel : PageModel
    {
        private readonly ArticleController _articleController;
        [BindProperty]
        public List<ArticleViewModel> Articles { get; set; }
        public ApproveArticlesModel(ApplicationDbContext ctx, IAuthorizationService authorizationService,UserManager<IdentityUser> userManager)
        {
            _articleController = new ArticleController(ctx, authorizationService,userManager);
        }
        public async Task<IActionResult> OnGet()
        {
            Articles = await _articleController.GetUnapprovedAsync();
            return Page();
        }
        [HttpPost]
        public async Task<IActionResult> OnPostApprove(int id)
        {
            await _articleController.ApproveArticle(id, User);
            return RedirectToPage("/ApproveArticles");

        }

    }
}