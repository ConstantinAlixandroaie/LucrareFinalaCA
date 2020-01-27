using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LucrareFinalaCA.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ArticleController _articleController;
        public List<ArticleViewModel> Articles { get; set; }
        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }
        public IndexModel(ApplicationDbContext ctx, IAuthorizationService authorizationService)
        {
            _articleController = new ArticleController(ctx, authorizationService);
        }

        public async Task<IActionResult> OnGet()
        {
            Articles = await _articleController.GetByLastAsync(searchString);
            return Page();
        }
    }

}
