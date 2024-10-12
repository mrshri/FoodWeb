using Food.Services.AuthAPI.Data;
using Food.Services.AuthAPI.Models;
using Food.Services.AuthAPI.Service;
using Food.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//configuring JwtOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("APISettings:JwtOptions"));

//Add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddScoped<IJWTTokenGeneratorService, JWTTokenGeneratorService>();
builder.Services.AddScoped<IAuthService, AuthService>();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}