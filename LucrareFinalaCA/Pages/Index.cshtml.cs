using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LucrareFinalaCA.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ArticleController _articleController;
        public List<ArticleViewModel> Articles { get; set; }
        public IndexModel(ApplicationDbContext ctx)
        {
            _articleController = new ArticleController(ctx);
        }

        public async Task<IActionResult> OnGet()
        {
            Articles = await _articleController.GetByLastAsync();
            return Page();
        }
    }

}
