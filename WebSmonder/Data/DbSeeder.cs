using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebSmonder.Data.Entities;
using WebSmonder.Models.Seeder;

namespace WebSmonder.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
            var context = scope.ServiceProvider.GetRequiredService<AppSmonderDbContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            context.Database.Migrate();

            if (!context.Categories.Any()) 
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                        var categoryEntities = mapper.Map<List<CategoryEntity>>(categories);
                        await context.AddRangeAsync(categoryEntities);
                        await context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                    }
                }
                else 
                {
                    Console.WriteLine("Not Found File Categories.json");
                }
            }
        }

    }
}
