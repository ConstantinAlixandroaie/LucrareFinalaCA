﻿using LucrareFinalaCA.Controllers;
using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LucrareFinalaCA.Pages
{
    public class AddArticleModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ArticleController _articleController;
        [BindProperty]
        public ArticleViewModel Article { get; set; }
        [BindProperty]
        [Required]
        public string categories { get; set; }
        string[] separator = { ",", " " };
        [BindProperty]
        [Required]
        public IFormFile Image { get; set; }
        public AddArticleModel(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _articleController = new ArticleController(ctx, authorizationService, userManager);
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Path.GetExtension(Image.FileName).Equals(".png") || Path.GetExtension(Image.FileName).Equals(".jpeg") || Path.GetExtension(Image.FileName).Equals(".jpg")
                || Path.GetExtension(Image.FileName).Equals(".gif") || Path.GetExtension(Image.FileName).Equals(".bmp"))
            {
                BinaryReader reader = new BinaryReader(Image.OpenReadStream());
                if(categories != null)
                Article.Categories = categories.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                Article.Author = _userManager.GetUserName(User);
                Article.Image = reader.ReadBytes((int)Image.Length);
                await _articleController.Add(Article, User);
                return RedirectToPage("/Index");
            }
            else
            {
                throw new Exception("You can only add pictures!");
            }
        }

    }
}
