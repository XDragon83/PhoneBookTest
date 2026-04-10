using Microsoft.EntityFrameworkCore;
using PhoneBook.Data.Context;
using PhoneBook.Repositories;
using PhoneBook.Repositories.Interfaces;
using PhoneBook.Services;
using PhoneBook.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add MVC - this returns an IMvcBuilder
var mvcBuilder = builder.Services.AddControllersWithViews();

// --- Localization Services ---
// 1. Add support for localization.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 2. Add View Localization services by chaining it to the IMvcBuilder
mvcBuilder.AddMvcLocalization();
// --- End Localization Services ---


// Add EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("fa")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});




// Build the application (this finalizes the service collection)
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- Apply the Request Localization Middleware ---
// This middleware uses the options configured above.
app.UseRequestLocalization();
// --- End Request Localization Middleware ---


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}/{id?}");

app.Run();
