using Domain.Profiles;
using Infrastructore.DbConexts;
using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using LibraryManagement.Filters;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

namespace LibraryManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/stackoverflow_log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LibraryDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:LibrarayManagement"]);
            });

            builder.Services.AddScoped<IRepositoryComm, RepositoryComm>();
            builder.Services.AddScoped<IRepositoryQu, RepositoryQu>();
            builder.Services.AddHttpClient<IRepositoryQu, RepositoryQu>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<LibraryActionFilter>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Host.UseSerilog();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles(); // index.html
            app.UseStaticFiles();  // wwwroot

            app.UseRouting();
            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.UseSerilogRequestLogging();


            app.MapControllers();

            app.Run();
        }
    }
}


// https://localhost:7203/swagger/index.html