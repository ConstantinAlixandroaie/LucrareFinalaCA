using System.Collections.Generic;
using System.Threading.Tasks;
using LucrareFinalaCA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LucrareFinalaCA.Controllers
{
    public abstract class BaseSiteController<T>
    {
        protected readonly ApplicationDbContext _ctx;
        protected readonly IAuthorizationService _authorizationService;

        public BaseSiteController(ApplicationDbContext ctx, IAuthorizationService authorizationService)
        {
            _ctx = ctx;
            _authorizationService = authorizationService;
        }

        public abstract Task<List<T>> GetAsync();
        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task Add(T vm, ClaimsPrincipal user);
        public abstract Task Edit(T vm, ClaimsPrincipal user);
        public abstract Task Delete(int id, ClaimsPrincipal user);
    }
}

