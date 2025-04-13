using Microsoft.EntityFrameworkCore;
using WebSmonder.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppSmonderDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

await app.SeedData();

app.Run(); //Запускає наш хост (Сервер) і ми бачимо консоль

// тут код писати не можна він працювати не буде 😒😒😒
