using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;

namespace TKR.App
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var service = builder.Services;

            service.AddSingleton<CoreService>();
            service.AddControllers();

            var app = builder.Build();

            var core = app.Services.GetService<CoreService>();

            app.Urls.Clear();
            app.Urls.Add($"http://{core.Config.serverInfo.bindAddress}:{core.Config.serverInfo.port}");

            if (!core.IsProduction())
            {
                service.AddEndpointsApiExplorer();
                service.AddSwaggerGen();
            }

            if (!core.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider($"{core.Resources.ResourcePath}/web/sfx"),
                RequestPath = "/sfx"
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider($"{core.Resources.ResourcePath}/web/music"),
                RequestPath = "/music"
            });

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers(); //app.MapEndpoints(endpoints => endpoints.MapControllers());

            app.Use(async (context, next) =>
            {
                core.Logger.Info($"Request: \"{context.Request.Path}\" from: {context.Connection.RemoteIpAddress}");
                await next();
            });

            app.Run();
        }
    }
}