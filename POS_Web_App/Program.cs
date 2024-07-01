using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using POS.Database;
using POS.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<POSDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDb"));

        builder.Services.AddScoped<HashingService>();
        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<SalesService>();
        builder.Services.AddScoped<SaleSessionService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "POS API", Version = "v1" });
        });

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

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "POS API");
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}