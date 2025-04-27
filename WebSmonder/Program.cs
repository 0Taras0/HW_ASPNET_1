using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebSmonder.Data;
using WebSmonder.Data.Entities.Identity;
using WebSmonder.Interfaces;
using WebSmonder.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppSmonderDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection")));

//Додаємо налаштування для UserManager, RoleManager, SignInManager - займається cookies
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppSmonderDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IImageService, ImageService>();

//у нас будуть View - це такі сторіки - де можна писати на C# Index.cshtml
//Велика перевага цих сторінок у тому, що вони перевіряються на c# і компілюються у збірку
//WebSmoder.dll - вихідний файл проекту.
//контролер - це клас на C#, який приймає запити від клієнта і виконує усію логіку роботи.
//Результати роботи (Model) контролера передають на View для відображення
builder.Services.AddControllersWithViews(); //Налаштування контейнерів, сервісів. Репозиторій.

var app = builder.Build();   //Створється збірка на основі даних налаштувать вище

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); //якщо виника помилка, то нас кидає на сторінку /Home/Error
}
app.UseRouting(); //Підтримка маршрутизаці - це коли ми можемо писатив в url localhost:2345/login

app.UseAuthorization();  //Підтримка авторизації - це будемо вивчали коли перейдемо до Identity

app.MapStaticAssets();  //Використання статичних файлі, тобто у нас буде працювати папка wwwroot

//Нашатування для маршрутів. У нас є контролери - Вони мають називатися HomeController
//При цьому враховується лише Home. Методи цього класу називаються Action - тобто обробники
//Для того, щоб при запуску сайту ми бачили, щось визивається згідного налаштувань HomeController
//і його метод Index при цьому може бути параметер у маршруті id - але там є знак питання, тобто
//може бути null
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Categories}/{action=Index}/{id?}")
    .WithStaticAssets();

var directory = builder.Configuration["ImagesDir"];
string stringPath = Path.Combine(Directory.GetCurrentDirectory(), directory);
Directory.CreateDirectory(stringPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(stringPath),
    RequestPath = $"/{directory}"
});

await app.SeedData();

app.Run(); //Запускає наш хост (Сервер) і ми бачимо консоль

// тут код писати не можна він працювати не буде 😒😒😒
