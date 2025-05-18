using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.Diagnostics;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Debug.WriteLine("Main()");
            //using (CProject._Me = new CProject(args))
            //{
                //CProject._Me.Run();
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                //builder.Services.AddControllers(); // 跟上面的差異?

                // 註冊 In-Memory Database (可改為 SQL Server 或其他 DB)
                // builder.Services.AddDbContext<CAppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                //builder.Services.AddDbContext<CAppDbContext>(options => options.UseMemoryCache("MyDatabase"));
                builder.Services.AddDbContext<CAppDbContext>(options
                    => options.UseInMemoryDatabase("MyDatabase")); // UseInMemoryDatabase 需下載 Microsoft.EntityFrameworkCore.InMemory 套件 

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
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapControllers();            // 設定 API 路由
                app.MapDefaultControllerRoute();  // 設定 MVC 路由 (UI)

                app.Run();
            //}
        }
    }
}
