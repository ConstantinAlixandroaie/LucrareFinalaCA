using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LucrareFinalaCA
{
    public class GetMyArticlesTestModel : PageModel
    {
        private readonly ArticleController _articleController;
        public List<ArticleViewModel> EditedArticles { get; set; }
        public List<ArticleViewModel> AuthorArticles { get; set; }
        public GetMyArticlesTestModel(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _articleController = new ArticleController(ctx, authorizationService, userManager);
        }
        public async Task<IActionResult> OnGet()
        {
            EditedArticles = await _articleController.GetByEditor(User);
            AuthorArticles = await _articleController.GetByAuthor(User);

            return Page();
        }
    }
}