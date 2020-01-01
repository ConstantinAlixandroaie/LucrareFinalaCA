using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LucrareFinalaCA
{
    public class EditArticleModel : PageModel
    {
        private readonly ArticleController _articleController;
        [BindProperty]
        public ArticleViewModel Article { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        public EditArticleModel(ApplicationDbContext ctx)
        {
            _articleController = new ArticleController(ctx);
        }
        public async Task<IActionResult> OnGet(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            Article = await _articleController.GetByIdAsync(id);

            return Page();


        }
        public async Task<IActionResult> OnPostEdit()
        {
            if (Image != null)
            {
                if (Path.GetExtension(Image.FileName).Equals(".png") || Path.GetExtension(Image.FileName).Equals(".jpeg") || Path.GetExtension(Image.FileName).Equals(".jpg")
                    || Path.GetExtension(Image.FileName).Equals(".gif") || Path.GetExtension(Image.FileName).Equals(".bmp"))
                {
                    BinaryReader reader = new BinaryReader(Image.OpenReadStream());
                    Article.Image = reader.ReadBytes((int)Image.Length);
                }
            }

            await _articleController.Edit(Article);
            return RedirectToPage("/Article");
        }
    }
}