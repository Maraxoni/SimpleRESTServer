using SimpleRESTServer.Controllers;
using System.Net;

namespace SimpleRESTServer
{
    public sealed class ServerMain
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<CustomHeaderFilter>();
            })
            .AddXmlDataContractSerializerFormatters();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<MessagesController>();

            var app = builder.Build();

            app.Urls.Add("https://localhost:8081");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Optional: Add BasicAuth middleware
            app.UseMiddleware<BasicAuthMiddleware>();

            app.UseRouting();
            app.UseAuthorization();

            // Enable Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}