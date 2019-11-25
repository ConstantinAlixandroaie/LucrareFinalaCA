using System.Collections.Generic;
using System.Threading.Tasks;
using LucrareFinalaCA.Data;

namespace LucrareFinalaCA.Controllers
{
    public abstract class BaseSiteController<T>
    {
        protected readonly ApplicationDbContext _ctx;

        public BaseSiteController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public abstract Task<List<T>> GetAsync();
        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task Add(T vm);
        public abstract Task Edit(T vm);
        public abstract Task Delete(int id);
    }
}

