using CalculatorWeb.Data;
using CalculatorWeb.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRpnCalculate, RpnCalculate>();
builder.Services.AddScoped<IRpnWithCache, RpnWithCache>();
builder.Services.AddScoped<IConvertions, Convertions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculate}/{action=Calculator}/{id?}");

app.Run();
