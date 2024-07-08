using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using POS.Database;
using POS.Repositories;
using POS.Services;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Azure.Cosmos;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Configure DbContext to use InMemory database for all entities
        builder.Services.AddDbContext<POSDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDb"));

        // Add other scoped services
        builder.Services.AddScoped<HashingService>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<SalesService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<UserService>();


        // Configure Key Vault
        var keyVaultName = builder.Configuration["KeyVaultName"];
        if (!string.IsNullOrEmpty(keyVaultName))
        {
            var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
            builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
        }

        // Retrieve the connection string from Key Vault
        var secretClient = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), new DefaultAzureCredential());
        KeyVaultSecret secret = secretClient.GetSecret(builder.Configuration["Secrets:CosmosConnectionString"]);
        var cosmosDbConnectionString = secret.Value;

        // Register CosmosClient as singleton
        builder.Services.AddSingleton(new CosmosClient(cosmosDbConnectionString));

        // Add DbContext for Cosmos DB
        builder.Services.AddDbContext<CosmosDbContext>(options => options.UseCosmos(cosmosDbConnectionString, "POS-DB"));

        // Register repositories and services
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ProductService>();

        // Add controllers
        builder.Services.AddControllers();

        // Configure Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "POS API", Version = "v1" });
        });


        // Build the app
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "POS API");
        });

        app.MapControllers();

        app.Run();
    }
}