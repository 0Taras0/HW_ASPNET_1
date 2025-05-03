using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebSmonder.Areas.Admin.Models.Users;
using WebSmonder.Constants;
using WebSmonder.Data.Entities.Identity;

namespace WebSmonder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController(UserManager<UserEntity> userManager, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await userManager.Users
                .ProjectTo<UserItemViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var user in model)
            {
                var entity = await userManager.FindByIdAsync(user.Id.ToString());
                if (entity == null)
                    continue;
                var roles = await userManager.GetRolesAsync(entity);
                user.Roles = roles.ToList();
            }

            return View(model);
        }
    }
}
