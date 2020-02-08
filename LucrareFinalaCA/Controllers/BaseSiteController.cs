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
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly ApplicationDbContext _ctx;
        protected readonly IAuthorizationService _authorizationService;

        public BaseSiteController(ApplicationDbContext ctx, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _ctx = ctx;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public abstract Task<List<T>> GetAsync();
        public abstract Task<T> GetByIdAsync(int id, ClaimsPrincipal user);
        public abstract Task Add(T vm, ClaimsPrincipal user);
        public abstract Task Edit(T vm, ClaimsPrincipal user);
        public abstract Task Delete(int id, ClaimsPrincipal user);
    }
}

