using AI_based_Secure_Code_Review_Tool.Data;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using System.ClientModel;

namespace AI_based_Secure_Code_Review_Tool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services for Key Vault
            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddSecretClient(new Uri(builder.Configuration["Azure:KeyVault:VaultUri"]))
                    .WithCredential(new DefaultAzureCredential());
            });

            // Configure OpenAI Client
            builder.Services.AddSingleton(serviceProvider =>
            {
                var secretClient = serviceProvider.GetRequiredService<SecretClient>();

                // Retrieve secrets from Azure Key Vault
                var endpoint = secretClient.GetSecret("OpenAI-Endpoint").Value.Value;
                var apiKey = secretClient.GetSecret("OpenAI-Key1").Value.Value;

                // Initialize OpenAIClientOptions
                var options = new OpenAIClientOptions();
                options.Endpoint = new Uri(endpoint);

                var credential = new ApiKeyCredential(apiKey);

                var client = new OpenAIClient(credential, options);

                return client;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
