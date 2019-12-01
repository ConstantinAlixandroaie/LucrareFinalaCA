using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Pages
{
    public class AddArticleModel : PageModel
    {
        private readonly ArticleController _articleController;
        [BindProperty]
        public ArticleViewModel Article { get; set; }
        [BindProperty]
        public string categories { get; set; }
        string[] separator = { ",", " " };
        public AddArticleModel(ApplicationDbContext ctx)
        {
            _articleController = new ArticleController(ctx);
        }
        
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Login");
            return Page();
        }
        public async Task<IActionResult> OnPostAdd()
        {
            Article.Categories = categories.Split(separator, StringSplitOptions.RemoveEmptyEntries); 
            Article.Author = User.Identity.Name;
            await _articleController.Add(Article);
            return RedirectToPage("/Index");
        }

    }
}
