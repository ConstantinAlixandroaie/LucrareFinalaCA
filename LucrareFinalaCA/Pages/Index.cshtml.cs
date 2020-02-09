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

namespace LucrareFinalaCA.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ArticleController _articleController;
        public List<ArticleViewModel> Articles { get; set; }
        public string searchString { get; set; }

        public IndexModel(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _articleController = new ArticleController(ctx, authorizationService,userManager);
        }

        public async Task<IActionResult> OnGet()
        {
            searchString = null;
            Articles = await _articleController.GetByLastAsync(searchString, searchString);
            return Page();
        }
    }

}
