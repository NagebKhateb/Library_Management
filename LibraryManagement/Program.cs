using Infrastructore.DbConexts;
using Infrastructore.Repositories.Commands;
using Infrastructore.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
