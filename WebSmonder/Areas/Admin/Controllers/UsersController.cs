using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebSmonder.Areas.Admin.Models.Users;
using WebSmonder.Constants;
using WebSmonder.Data.Entities.Identity;
using WebSmonder.Interfaces;
using WebSmonder.Services;

namespace WebSmonder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController(UserManager<UserEntity> userManager, IMapper mapper, IImageService imageService) : Controller
    {
        [HttpGet]
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.Users
                .ProjectTo<UserItemEditModel>(mapper.ConfigurationProvider)
                .FirstAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            user.Roles = (List<string>)await userManager.GetRolesAsync(await userManager.FindByIdAsync(user.Id.ToString()));
            ViewBag.Roles = new SelectList(new[] { Roles.Admin, Roles.User });
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserItemEditModel model)
        {
            ModelState.Remove(nameof(model.Image));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            string image = user.Image;
            user = mapper.Map(model, user);
            user.Image = image;
            if (model.Image != null)
            {
                await imageService.DeleteImageAsync(user.Image);
                user.Image = await imageService.SaveImageAsync(model.Image);
            }
            await userManager.AddToRolesAsync(user, model.Roles);
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }
}
