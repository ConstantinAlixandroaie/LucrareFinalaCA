﻿using LucrareFinalaCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LucrareFinalaCA.Data;
using Microsoft.EntityFrameworkCore;

namespace LucrareFinalaCA.Controllers
{
    public class ArticleController : BaseSiteController<ArticleViewModel>
    {
        public ArticleController(ApplicationDbContext ctx) : base(ctx)
        {
        }
        public override async Task Add(ArticleViewModel vm)
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

            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var article = new Article()
                    {
                        Title = vm.Title,
                        ImgUrl = vm.ImgUrl,
                        ArticleText = vm.ArticleText,
                        Author = vm.Author,
                        IssueDate = DateTime.Now,
                    };
                    _ctx.Articles.Add(article);
                    await _ctx.SaveChangesAsync();

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
        public override async Task Delete(int id)
        {
            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
                throw new ArgumentException($"An Article with the given ID = '{id}' was not found ");
            _ctx.Articles.Remove(article);
            await _ctx.SaveChangesAsync();
        }

        public override async Task Edit(ArticleViewModel vm)
        {
            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (article == null)
                throw new ArgumentException($"An Article with the given ID = '{vm.Id}' was not found ");
            if (vm.Title != null)
                article.Title = vm.Title;
            if (vm.ArticleText != null)
                article.ArticleText = vm.ArticleText;
            if (vm.Title != null || vm.ArticleText != null)
                article.EditedDate = DateTime.Now;
        }

        public override async Task<List<ArticleViewModel>> GetAsync()
        {
            var rv = new List<ArticleViewModel>();
            var articles = await _ctx.Articles.ToListAsync();
            foreach (var article in articles)
            {
                var vm = new ArticleViewModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Author = article.Author,
                    ArticleText = article.ArticleText,
                    IssueDate = article.IssueDate,
                    EditedDate = article.EditedDate,
                };
                rv.Add(vm);
            }
            return rv;
        }

        public override async Task<ArticleViewModel> GetByIdAsync(int id)
        {
            var article = await _ctx.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                throw new ArgumentException($"An Article with the given ID = '{id}' was not found ");
            }
            var rv = new ArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Author = article.Author,
                ArticleText = article.ArticleText,
                IssueDate = article.IssueDate,
                EditedDate = article.EditedDate,
            };
            return rv;
        }
    }
}