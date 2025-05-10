using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSmonder.Constants;
using WebSmonder.Data.Entities;
using WebSmonder.Data.Entities.Identity;
using WebSmonder.Interfaces;
using WebSmonder.Models.Seeder;
using WebSmonder.SMTP;

namespace WebSmonder.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
            var context = scope.ServiceProvider.GetRequiredService<AppSmonderDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var smtpService = scope.ServiceProvider.GetRequiredService<ISMTPService>();

            //webApplication.Use(async (context, next) =>
            //{
            //    var host = context.Request.Host;

            //    Message message = new Message
            //    {
            //        Body = $"Додаток успішно запущено {DateTime.Now}",
            //        Subject = $"Запуск додатку {host}",
            //        To = "mehiwar472@nutrv.com"
            //    };
            //    await smtpService.SendMessage(message);
            //    Console.WriteLine($"------------{host}---------------");

            //    await next.Invoke();
            //});

            context.Database.Migrate();

            await SeedCategories(context, mapper, imageService);

            await SeedRoles(context, mapper, scope);

            await SeedUsers(context, mapper, userManager, imageService);

            await SeedProducts(context, mapper, imageService);
        }

        private static async Task SeedProducts(AppSmonderDbContext context, IMapper mapper, IImageService imageService)
        {
            if (!context.Products.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Products.json");

                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var products = JsonSerializer.Deserialize<List<SeederProductModel>>(jsonData);

                        foreach (var product in products)
                        {
                            var category = await context.Categories
                                .FirstOrDefaultAsync(c => c.Name == product.CategoryName);

                            if (category == null)
                            {
                                Console.WriteLine($"Category '{product.CategoryName}' not found for product '{product.Name}'");
                                continue;
                            }

                            var productEntity = new ProductEntity
                            {
                                Name = product.Name,
                                Description = product.Description,
                                CategoryId = category.Id,
                                ProductImages = new List<ProductImageEntity>()
                            };

                            int priority = 0;

                            for (int i = 0; i < product.Images.Count; i++)
                            {
                                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "SeedImages", "Products", product.Images[i]);
                                var formFile = await LoadImageAsFormFileAsync(imagePath, product.Images[i]);
                                if (formFile == null)
                                {
                                    Console.WriteLine($"Image file not found: {product.Images[i]}");
                                    continue;
                                }

                                var savedImageUrl = await imageService.SaveImageAsync(formFile);
                                productEntity.ProductImages.Add(new ProductImageEntity
                                {
                                    Name = savedImageUrl,
                                    Priotity = priority++
                                });
                            }

                            await context.Products.AddAsync(productEntity);
                        }

                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Product Data: {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Products.json file not found");
                }
            }
        }

        private async static Task SeedCategories(AppSmonderDbContext context, IMapper mapper, IImageService imageService)
        {
            if (!context.Categories.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);

                        foreach (var model in categories)
                        {
                            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "SeedImages", "Categories", model.Image);
                            var formFile = await LoadImageAsFormFileAsync(imagePath, model.Image);
                            if (formFile == null)
                            {
                                Console.WriteLine($"Image file not found: {model.Image}");
                                continue;
                            }

                            var entity = mapper.Map<CategoryEntity>(model);

                            entity.ImageUrl = await imageService.SaveImageAsync(formFile);

                            await context.Categories.AddAsync(entity);
                        }

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

        private async static Task<IFormFile> LoadImageAsFormFileAsync(string imagePath, string imageName)
        {
            var fileInfo = new FileInfo(imagePath);

            if (!File.Exists(imagePath))
            {
                return null;
            }

            var memoryStream = new MemoryStream(await File.ReadAllBytesAsync(imagePath));

            return new FormFile(memoryStream, 0, memoryStream.Length, "ImageFile", imageName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + fileInfo.Extension.Trim('.')
            };
        }

        private async static Task SeedRoles(AppSmonderDbContext context, IMapper mapper, IServiceScope scope)
        {
            if (!context.Roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                var admin = new RoleEntity { Name = Roles.Admin };
                var result = await roleManager.CreateAsync(admin);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Роль {Roles.Admin} створено успішно");
                }
                else
                {
                    Console.WriteLine($"Помилка створення ролі:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }

                var user = new RoleEntity { Name = Roles.User };
                result = await roleManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Роль {Roles.User} створено успішно");
                }
                else
                {
                    Console.WriteLine($"Помилка створення ролі:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }
        }

        private async static Task SeedUsers(AppSmonderDbContext context, IMapper mapper, UserManager<UserEntity> userManager, IImageService imageService)
        {
            if (!context.Users.Any())
            {
                await SeedAmin(context, userManager);
                await SeedUsersFromJson(context, mapper, userManager, imageService);
            }
        }

        private async static Task SeedUsersFromJson(AppSmonderDbContext context, IMapper mapper, UserManager<UserEntity> userManager, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    foreach (var model in users)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "SeedImages", "Users", model.Image);
                        var formFile = await LoadImageAsFormFileAsync(imagePath, model.Image);

                        if (formFile == null)
                        {
                            Console.WriteLine($"Image file not found: {model.Image}");
                            continue;
                        }

                        var entity = mapper.Map<UserEntity>(model);
                        entity.Image = await imageService.SaveImageAsync(formFile);
                        var result = await userManager.CreateAsync(entity, model.Password);

                        if (result.Succeeded)
                        {
                            Console.WriteLine($"Користувача успішно створено {entity.LastName} {entity.FirstName}!");
                            await userManager.AddToRoleAsync(entity, Roles.User);
                        }
                        else
                        {
                            Console.WriteLine($"Помилка створення користувача:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"- {error.Code}: {error.Description}");
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }
        private async static Task SeedAmin(AppSmonderDbContext context, UserManager<UserEntity> userManager)
        {
            string email = "admin@gmail.com";
            var user = new UserEntity
            {
                UserName = email,
                Email = email,
                FirstName = "Адмін",
                LastName = "Батькович"
            };

            var result = await userManager.CreateAsync(user, "123456");
            if (result.Succeeded)
            {
                Console.WriteLine($"Користувача успішно створено {user.LastName} {user.FirstName}!");
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
            else
            {
                Console.WriteLine($"Помилка створення користувача:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
            }
        }
    }
}
