using AspNetCoreHero.ToastNotification.Extensions;
using ClothesDG.Data.Data;
using ClothesDG.Data.Data.Models;
using ClothesDG.Extensions;
using ClothesDG.Middlewares;
using ClothesDG.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ClothesDGContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
    .AddRoles<ApplicationRole>()
    .AddErrorDescriber<IdentityErrorDescriberProvider>()
    .AddEntityFrameworkStores<ClothesDGContext>();


builder.Services.AddControllersWithViews(
    options =>
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }
);

builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.Seed();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseCustomEndpoints();

app.UseNotyf();
app.MapRazorPages();

app.Run();
