using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using WebSmonder.Data;
using WebSmonder.Data.Entities;
using WebSmonder.Interfaces;
using WebSmonder.Models.Category;

namespace WebSmonder.Controllers;


//public class CategoriesController : Controller
//{
//    private readonly AppSmonderDbContext _context;
//    private readonly IMapper _mapper;
//    public CategoriesController(AppSmonderDbContext context, IMapper maper)
//    {
//        _context = context;
//        _mapper = maper;  
//    }

//    public IActionResult Index() //Це будь-який web результат - View - сторінка, Файл, PDF, Excel
//    {
//        var model = _mapper.ProjectTo<CategoryItemViewModel>(_context.Categories).ToList();
//        return View(model);
//    }
//}


public class CategoriesController(AppSmonderDbContext context, IMapper mapper, IImageService imageService) : Controller
{

    public IActionResult Index() //Це будь-який web результат - View - сторінка, Файл, PDF, Excel
    {
        var model = mapper.ProjectTo<CategoryItemViewModel>(context.Categories).ToList();
        return View(model);
    }

    [HttpGet] // Тепер він працює методом GET - це щоб побачити форму
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost] //Тепер він працює методом GET - це щоб побачити форму
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        var entity = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name);
        if (entity != null)
        {
            ModelState.AddModelError("Name", "Така категорія уже є!!!");
            return View(model);
        }

        entity = mapper.Map<CategoryEntity>(model);
        entity.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
        await context.Categories.AddAsync(entity);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await context.Categories.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        //Динамічна колекція, яка зберігає динамічні дані, які можна використати на View
        //ViewBag.ImageName = item.ImageUrl;

        var model = mapper.Map<CategoryEditViewModel>(item);
        return View(model);
    }

    [HttpPost] //Тепер він працює методом GET - це щоб побачити форму
    public async Task<IActionResult> Edit(CategoryEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (existing == null)
        {
            return NotFound();
        }

        var duplicate = await context.Categories
            .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
        if (duplicate != null)
        {
            ModelState.AddModelError("Name", "Another category with this name already exists");
            return View(model);
        }

        existing = mapper.Map(model, existing);
        if (model.ImageFile != null)
        {
            await imageService.DeleteImageAsync(existing.ImageUrl);
            existing.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
        }

        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        CategoryDeleteViewModel model = mapper.Map<CategoryDeleteViewModel>(item);
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await context.Categories.FindAsync(id);
        if (item != null)
        {
            context.Categories.Remove(item);
            await context.SaveChangesAsync();
            await imageService.DeleteImageAsync(item.ImageUrl);
        }
        return RedirectToAction(nameof(Index));
    }


}
