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
        private readonly UserManager<IdentityUser> _userManager;
        [BindProperty]
        public List<ArticleViewModel> Articles { get; set; }
        [BindProperty]
        public ArticleViewModel Article { get; set; }
        [BindProperty]
        public Article article { get; set; }
        protected IAuthorizationService AuthorizationService { get; }
        protected ApplicationDbContext _ctx { get; }
        [BindProperty]
        public bool IsbyId { get; set; }

        public ArticleModel(ApplicationDbContext ctx, IAuthorizationService authorizationService,UserManager<IdentityUser> UserManager)
        {
            _articleController = new ArticleController(ctx);
            _ctx = ctx;
            AuthorizationService = authorizationService;
            _userManager = UserManager;

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

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var role = await _userManager.GetRolesAsync(user);

            //CRUD will be modified to take a user view model as a parameter and work only for the appropriate users.
            article = await _ctx.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, article, ArticleOperations.Delete);


            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            await _articleController.Delete(id);

            return RedirectToPage("./Index");
        }
    }
}