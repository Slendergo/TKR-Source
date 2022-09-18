using Microsoft.AspNetCore.Mvc.Formatters;

namespace TKR.App
{
    public sealed class Program
    {
        public static string ResourcePath { get; private set; }

        public static void Main(string[] args)
        {
            ResourcePath = $"{Environment.CurrentDirectory}/web";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddMvc(setupAction =>
            {
                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddXmlSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("sfx", "{controller}/{type}");
                endpoints.MapControllerRoute("music", "{controller}/{type}");
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}