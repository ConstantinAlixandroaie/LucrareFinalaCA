using LucrareFinalaCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using LucrareFinalaCA.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LucrareFinalaCA.Controllers
{
    public class ArticleController : BaseSiteController<ArticleViewModel>
    {
        public ArticleController(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager) : base(ctx, authorizationService, userManager)
        {
        }

        public override async Task Add(ArticleViewModel vm, ClaimsPrincipal user)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            if (vm.Title == null)
            {
                throw new ArgumentException("Title is null");
            }
            if (vm.ArticleText == null)
            {
                throw new ArgumentException("Article text is null");
            }

            //get categires from database
            var availablecategories = await _ctx.Categories.ToListAsync();
            //create a list of string that contains only the category name
            List<string> availablecategorynames = new List<string>();
            //populate the list
            foreach (var category in availablecategories)
            {
                availablecategorynames.Add(category.Name);
            }
            // create an array of category names that can be added. except selects all the categories in the first set, 
            // in our case vm.Categories -which come from the interface, that do not exist in the second set in our case
            // availablecategorynames whcih are read from the databse.

            var newcategs = vm.Categories.Except(availablecategorynames.ToArray());

            //var isAuthorized = await _authorizationService.AuthorizeAsync(user, article, ArticleOperations.Create);
            //if (!isAuthorized.Succeeded)
            //{
            //    throw new ArgumentException("The currently logged in user is not allowed to add articles");
            //}

            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var article = new Article()
                    {
                        Title = vm.Title,
                        Image = vm.Image,
                        ArticleText = vm.ArticleText,
                        Author = vm.Author,
                        IssueDate = DateTime.Now,
                        ApprovedStatus = vm.ApprovedStatus
                    };
                    _ctx.Articles.Add(article);
                    await _ctx.SaveChangesAsync();

                    var isAuthorized = await _authorizationService.AuthorizeAsync(user, article, ArticleOperations.Create);
                    if (!isAuthorized.Succeeded)
                    {
                        throw new ArgumentException("The currently logged in user is not allowed to add articles");
                    }

                    foreach (var categ in newcategs)
                    {
                        var category = new Category()
                        {
                            Name = categ,
                        };
                        _ctx.Categories.Add(category);
                        await _ctx.SaveChangesAsync();
                    }
                    //Get all the categories that we should map from the database.
                    //Compare all the values from the DB with the ones from the interface 
                    var categoriesToMap = await (from categs in _ctx.Categories
                                                 where vm.Categories.Contains(categs.Name)
                                                 select categs).ToListAsync();
                    foreach (var categ in categoriesToMap)
                    {
                        var artcategmapping = new ArticleCategoryMapping()
                        {
                            ArtId = article.Id,
                            CategId = categ.Id,
                        };
                        _ctx.ArticleCategoryMappings.Add(artcategmapping);
                        await _ctx.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public override async Task Delete(int id, ClaimsPrincipal user)
        {

            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                throw new ArgumentException($"An Article with the given ID = '{id}' was not found ");
            }
            var isAuthorized = await _authorizationService.AuthorizeAsync(user, article, ArticleOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                throw new ArgumentException("The currently logged in user is not allowed to delete that article.");
            }
            _ctx.Articles.Remove(article);
            await _ctx.SaveChangesAsync();
        }

        public override async Task Edit(ArticleViewModel vm, ClaimsPrincipal user)
        {
            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == vm.Id);
            var userId = _userManager.GetUserId(user);
            if (article == null)
                throw new ArgumentException($"An Article with the given ID = '{vm.Id}' was not found ");

            var isAuthorized = await _authorizationService.AuthorizeAsync(user, article, ArticleOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                throw new ArgumentException("The currently logged in user is not allowed to edit that article.");
            }
            if (vm.Title != null && vm.Title != article.Title)
                article.Title = vm.Title;
            if (vm.ArticleText != null && vm.ArticleText != article.ArticleText)
                article.ArticleText = vm.ArticleText;
            if (vm.Image != null && vm.Image != article.Image)
                article.Image = vm.Image;
            if ((vm.Title != null && vm.Title != article.Title) || (vm.ArticleText != null && vm.ArticleText != article.ArticleText) || (vm.Image != null && vm.Image != article.Image))
                article.EditedDate = DateTime.Now;
            _ctx.Attach(article).State = EntityState.Modified;
            ArticleEditorMapping articleEditorMapping = new ArticleEditorMapping()
            {
                ArticleId = article.Id,
                UserId = userId
            };
            _ctx.Add(articleEditorMapping);
            await _ctx.SaveChangesAsync();
        }

        public override async Task<List<ArticleViewModel>> GetAsync()
        {
            var rv = new List<ArticleViewModel>();
            var articles = await _ctx.Articles.ToListAsync();
            foreach (var article in articles)
            {
                if (article.ApprovedStatus)
                {
                    var vm = new ArticleViewModel()
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Image = article.Image,
                        Author = article.Author,
                        ArticleText = article.ArticleText,
                        IssueDate = article.IssueDate,
                        EditedDate = article.EditedDate,
                    };
                    rv.Add(vm);
                }
            }
            return rv;
        }

        public override async Task<ArticleViewModel> GetByIdAsync(int id, ClaimsPrincipal user)
        {
            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                throw new ArgumentException($"An Article with the given ID = '{id}' was not found ");
            }
            var isAuthorised = await _authorizationService.AuthorizeAsync(user, article, ArticleOperations.Approve);

            if (!article.ApprovedStatus & (!isAuthorised.Succeeded || article.Author != _userManager.GetUserName(user)))
            {
                throw new ArgumentException($"The article with the given Id='{id}' has yet to be approved!");
            }
            var rv = new ArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Image = article.Image,
                Author = article.Author,
                ArticleText = article.ArticleText,
                IssueDate = article.IssueDate,
                EditedDate = article.EditedDate,
                ApprovedStatus = article.ApprovedStatus
            };
            return rv;
        }
        public async Task<List<ArticleViewModel>> GetByLastAsync(string searchString, string category)
        {
            var rv = new List<ArticleViewModel>();
            var articles = await (from arts in _ctx.Articles
                                  orderby arts.IssueDate descending
                                  select arts).Take(9).ToListAsync();
            var searcharticles = from a in _ctx.Articles
                                 select a;

            if (!string.IsNullOrEmpty(searchString) || !string.IsNullOrEmpty(category))
            {
                if (!string.IsNullOrEmpty(category))
                {
                    searcharticles = from art in _ctx.Articles
                                     join maps in _ctx.ArticleCategoryMappings on art.Id equals maps.ArtId
                                     where maps.CategId == Int16.Parse(category)
                                     select art;
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    searcharticles = searcharticles.Where(s => s.Title.Contains(searchString) || s.ArticleText.Contains(searchString));
                }
                foreach (var art in searcharticles)
                {
                    if (art.ApprovedStatus)
                    {
                        var vm = new ArticleViewModel()
                        {
                            Id = art.Id,
                            Title = art.Title,
                            Image = art.Image,
                            Author = art.Author,
                            ArticleText = art.ArticleText,
                            IssueDate = art.IssueDate,
                            EditedDate = art.EditedDate,
                        };
                        rv.Add(vm);
                    }
                }
            }
            else
                foreach (var art in articles)
                {
                    if (art.ApprovedStatus)
                    {
                        var vm = new ArticleViewModel()
                        {
                            Id = art.Id,
                            Title = art.Title,
                            Image = art.Image,
                            Author = art.Author,
                            ArticleText = art.ArticleText,
                            IssueDate = art.IssueDate,
                            EditedDate = art.EditedDate,
                        };
                        rv.Add(vm);
                    }
                }
            return rv;
        }

        public async Task ApproveArticle(int id, ClaimsPrincipal user)
        {
            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                throw new ArgumentException($"An Article with the given ID = '{id}' was not found ");
            }
            var isAuthorized = await _authorizationService.AuthorizeAsync(user, article, ArticleOperations.Approve);
            if (!isAuthorized.Succeeded)
            {
                throw new ArgumentException("The currently logged in user is not allowed to delete that article.");
            }
            article.ApprovedStatus = true;
            _ctx.Attach(article).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<ArticleViewModel>> GetUnapprovedAsync()
        {
            var rv = new List<ArticleViewModel>();
            var articles = await _ctx.Articles.ToListAsync();
            foreach (var article in articles)
            {
                if (!article.ApprovedStatus)
                {
                    var vm = new ArticleViewModel()
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Image = article.Image,
                        Author = article.Author,
                        ArticleText = article.ArticleText,
                        IssueDate = article.IssueDate,
                        EditedDate = article.EditedDate,
                    };
                    rv.Add(vm);
                }
            }
            return rv;
        }
        public async Task<List<ArticleViewModel>> GetByEditor(ClaimsPrincipal user)
        {
            var rv = new List<ArticleViewModel>();
            var userId = _userManager.GetUserId(user);

            var query = await (from art in _ctx.Articles
                               join maps in _ctx.ArticleEditorMappings on art.Id equals maps.ArticleId
                               where maps.UserId == userId
                               select art).ToListAsync();

            foreach (var article in query)
            {
                var vm = new ArticleViewModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Image = article.Image,
                    Author = article.Author,
                    ArticleText = article.ArticleText,
                    IssueDate = article.IssueDate,
                    EditedDate = article.EditedDate,
                };
                rv.Add(vm);
            }
            return rv;
        }
        public async Task<List<ArticleViewModel>> GetByAuthor(ClaimsPrincipal user)
        {
            var rv = new List<ArticleViewModel>();
            var userId = _userManager.GetUserName(user);
            var articles = await (from art in _ctx.Articles
                                  where art.Author == userId
                                  select art).ToListAsync();

            foreach (var article in articles)
            {
                var vm = new ArticleViewModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Image = article.Image,
                    Author = article.Author,
                    ArticleText = article.ArticleText,
                    IssueDate = article.IssueDate,
                    EditedDate = article.EditedDate,
                };
                rv.Add(vm);
            }
            return rv;
        }
    }
}
