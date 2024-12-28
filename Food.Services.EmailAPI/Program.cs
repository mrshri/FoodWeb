
using Food.Services.EmailAPI.Data;
using Food.Services.EmailAPI.Extension;
using Food.Services.EmailAPI.Messaging;
using Food.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;


namespace Food.Services.EmailAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services for the database connection string
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddSingleton<IAzureServiceBusConsumer,AzureServiceBusConsumer>();

            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

            // Add services to the container.
            builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

            builder.Services.AddControllers();
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.UseAzureServiceBusConsumer();
            ApplyMigration();


            app.Run();

            void ApplyMigration()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
            }
        }
    }
}
