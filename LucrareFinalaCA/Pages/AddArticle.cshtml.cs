using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
        [BindProperty]
        public IFormFile Image { get; set; }
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
            if (Path.GetExtension(Image.FileName) != ".png" || Path.GetExtension(Image.FileName) != ".jpeg" || Path.GetExtension(Image.FileName) != ".jpg"
                || Path.GetExtension(Image.FileName) != ".gif" || Path.GetExtension(Image.FileName) != ".bmp")
            {
                throw new Exception("YOu can only add pictures!");
            }
            else
            {
                BinaryReader reader = new BinaryReader(Image.OpenReadStream());
                Article.Categories = categories.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                Article.Author = User.Identity.Name;
                Article.Image = reader.ReadBytes((int)Image.Length);
                await _articleController.Add(Article);
                return RedirectToPage("/Index");
            }
        }

    }
}
