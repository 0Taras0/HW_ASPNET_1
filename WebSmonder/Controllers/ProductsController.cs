using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebSmonder.Data;
using WebSmonder.Data.Entities;
using WebSmonder.Interfaces;
using WebSmonder.Models.Helpers;
using WebSmonder.Models.Product;

namespace WebSmonder.Controllers;
public class ProductsController(AppSmonderDbContext context,
        IMapper mapper, IImageService imageService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(ProductSearchViewModel searchModel)
    {
        ViewBag.Title = "Продукти";

        searchModel.Categories = await mapper.ProjectTo<SelectItemViewModel>(context.Categories).ToListAsync();

        searchModel.Categories.Insert(0, new SelectItemViewModel
        {
            Id = 0,
            Name = "Всі категорії"
        });

        var query = context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(searchModel.Name))
        {
            string textSearch = searchModel.Name.Trim();
            query = query.Where(x => x.Name.ToLower().Contains(textSearch.ToLower()));
        }

        if (!string.IsNullOrEmpty(searchModel.Description))
        {
            string textSearch = searchModel.Description.Trim();
            query = query.Where(x => x.Description.ToLower().Contains(textSearch.ToLower()));
        }

        if (searchModel.CategoryId > 0)
        {
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);
        }

        var model = new ProductListViewModel();

        //Відбір тих елементів, які відображаються на сторінці
        int count = query.Count();
        query = query
            .Skip((searchModel.Pagination.Page - 1) * searchModel.Pagination.PageSize)
            .Take(searchModel.Pagination.PageSize);
        model.Products = mapper.ProjectTo<ProductItemViewModel>(query).ToList();
        model.Search = searchModel;

        model.Search.Pagination = new PaginationItemViewModel
        {
            Page = searchModel.Pagination.Page,
            PageSize = searchModel.Pagination.PageSize,
            TotalCount = count
        };

        model.Count = query.Count();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Title = "Створити продукт";
        ViewBag.Categories = context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductItemCreateModel model)
    {
        var existingProduct = await context.Products.SingleOrDefaultAsync(x => x.Name == model.Name);

        if (existingProduct != null)
        {
            ModelState.AddModelError("Name", "Такий продукт вже є!!!");
            return View(model);
        }

        var productEntity = mapper.Map<ProductEntity>(model);

        productEntity.Category = await context.Categories
            .SingleOrDefaultAsync(x => x.Name == model.CategoryName);

        var savedImages = await Task.WhenAll(
            model.Images.Select(async image => new ProductImageEntity
            {
                Name = await imageService.SaveImageFromBase64Async(image.Name),
                Priotity = image.Priority
            })
        );

        productEntity.ProductImages = savedImages.ToList();
        
        productEntity.Description = await ProcessDescriptionImagesAsync(
            productEntity.Description,
            productEntity.DescriptionImages
        );

        context.Products.Add(productEntity);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);
        product.ProductImages = await context.Products
            .Include(x => x.ProductImages)
            .Where(x => x.Id == id)
            .Select(x => x.ProductImages)
            .FirstOrDefaultAsync();
        var model = mapper.Map<DeleteProductViewModel>(product);
        if (product == null)
        {
            return NotFound();
        }

        return View(model); 
    }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await context.Products
            .Include(x => x.ProductImages)
            .SingleOrDefaultAsync(x => x.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        foreach (var image in product.ProductImages)
        {
            await imageService.DeleteImageAsync(image.Name);
        }
        foreach (var image in product.DescriptionImages)
        {
            await imageService.DeleteImageAsync(image);
        }
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private async Task<string> ProcessDescriptionImagesAsync(
    string descriptionHtml,
    ICollection<string> savedImagePaths)
    {
        var pattern = @"src\s*=\s*[""'](?<url>https?[^""']+)[""']";

        var matches = Regex.Matches(descriptionHtml, pattern);
        var replacedHtml = descriptionHtml;

        foreach (Match match in matches)
        {
            var originalUrl = match.Groups["url"].Value;

            if (string.IsNullOrWhiteSpace(originalUrl))
                continue;

            try
            {
                var newUrl = await imageService.SaveImageFromUrlAsync(originalUrl);
                savedImagePaths.Add(newUrl);

                newUrl = $"/images/1200_{newUrl}";

                replacedHtml = ReplaceFirst(replacedHtml, originalUrl, newUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося зберегти {originalUrl}: {ex.Message}");
            }
        }

        return replacedHtml;
    }
    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search, StringComparison.Ordinal);
        if (pos < 0)
            return text;
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

}
