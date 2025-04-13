using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSmonder.Data;
using WebSmonder.Data.Entities;
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


public class CategoriesController(AppSmonderDbContext context, IMapper mapper) : Controller
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

    [HttpPost] // Тепер він працює методом POST - це щоб відправити форму
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        var item = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name);
        if (item != null)
        {
            ModelState.AddModelError("Name", "Категорія з такою назвою вже існує");
            return View(model);
        }

        item = mapper.Map<CategoryEntity>(model);
        await context.Categories.AddAsync(item);
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
        var model = mapper.Map<CategoryEditViewModel>(item);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            var item = await context.Categories.FindAsync(model.Id);
            if (item == null)
                return View(model);

            mapper.Map(model, item);

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
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
        }
        return RedirectToAction(nameof(Index));
    }
}
