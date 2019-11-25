using LucrareFinalaCA.Data;
using LucrareFinalaCA.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrareFinalaCA.Controllers
{
    public class CategoryController : BaseSiteController<CategoryViewModel>
    {

        public CategoryController(ApplicationDbContext ctx) : base(ctx)
        {

        }
        public override async Task Add(CategoryViewModel vm)
        {
            //check if the vm(view model ) is null. 
            //create the category only if the name in the view model is not null.
            if(vm==null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            if (vm.Name==null)
            {
                throw new ArgumentException("Category Name cannot be null!");
            }
            var category = new Category()
            {
                Name = vm.Name,
            };
            _ctx.Categories.Add(category);
            await _ctx.SaveChangesAsync();
        }

        public override Task Delete(int id)
        {
            //never used
            throw new NotImplementedException();
        }

        public override Task Edit(CategoryViewModel vm)
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
            foreach (var category  in categories)
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
