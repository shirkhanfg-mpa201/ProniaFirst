using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Pronia.Contexts;
using System;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
