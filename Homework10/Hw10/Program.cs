// dotcover disable

using System.Diagnostics.CodeAnalysis;
using Hw10.Configuration;
using Hw10.DbModels;
using Microsoft.EntityFrameworkCore;

public class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services
            .AddMathCalculator()
            .AddCachedMathCalculator();
        
        builder.Services.AddMemoryCache();

        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Calculator}/{action=Index}/{id?}");
        app.Run();
    }
}