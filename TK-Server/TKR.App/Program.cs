using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.FileProviders;
using System.Net;
using TKR.App.Controllers;
using TKR.App.Database;

namespace TKR.App
{
    public sealed class Program
    {
        public static string ResourcePath { get; private set; }

        public static void Main(string[] args)
        {
            ResourcePath = $"{Environment.CurrentDirectory}/resources/web";

            var builder = WebApplication.CreateBuilder(args);
            var service = builder.Services;

            service.AddSingleton<RedisDatabase>();

            service.AddControllers();

            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider($"{ResourcePath}/sfx"),
                RequestPath = "/sfx"
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider($"{ResourcePath}/music"),
                RequestPath = "/music"
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers(); //app.MapEndpoints(endpoints => endpoints.MapControllers());

            // create instance of DB now
            _ = app.Services.GetService<RedisDatabase>();

            app.Run();
        }
    }
}