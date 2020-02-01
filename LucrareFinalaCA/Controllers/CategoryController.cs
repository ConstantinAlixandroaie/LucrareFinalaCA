using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LucrareFinalaCA.Controllers
{
    public class CategoryController : BaseSiteController<CategoryViewModel>
    {

        public CategoryController(ApplicationDbContext ctx,IAuthorizationService authorizationService, UserManager<IdentityUser> userManager) : base(ctx,authorizationService, userManager)
        {

        }
        string[] separator = { ",", " " };
        public override Task Add(CategoryViewModel vm,ClaimsPrincipal user)
        {
            //never used.
            //Category add fucntion is performed by the article controller
            throw new NotImplementedException();
        }

        public override Task Delete(int id, ClaimsPrincipal user)
        {
            //never used
            throw new NotImplementedException();
        }

        public override Task Edit(CategoryViewModel vm, ClaimsPrincipal user)
        {
            //never used
            throw new NotImplementedException();
        }

        public override async Task<List<CategoryViewModel>> GetAsync()
        {
            //prepare a list to take our desired items from the database
            var rv = new List<CategoryViewModel>();
            //ask for all the categories from the database
            var categories = await _ctx.Categories.ToListAsync();
            //go through each element in our database requested values list and create a new view model for each one. 
            //after which we save each view moel in the previously created list.
            foreach (var category in categories)
            {
                var vm = new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                rv.Add(vm);
            }
            //return out list. it can be used by the client to display the data on views.
            return rv;
        }

        public override Task<CategoryViewModel> GetByIdAsync(int id)
        {
            //never used
            throw new NotImplementedException();
        }
    }
}
