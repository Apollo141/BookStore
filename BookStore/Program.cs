 using Infastructure.Seed;
using Application.Interfaces;
using Application.Services;
using BookStore.Infrastructure;
using Infastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookStore.Middleware;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

 //builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();


var jwt = builder.Configuration.GetSection("Jwt"); var key = Encoding.UTF8.GetBytes(jwt["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwt["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwt["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy =>
        policy.RequireRole("User", "Admin"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await app.SeedDataAsync();

// 4.3 Seed roles
using var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider
                       .GetRequiredService<RoleManager<IdentityRole>>();
foreach (var role in new[] { "Admin", "User" })
    if (!await roleManager.RoleExistsAsync(role))
        await roleManager.CreateAsync(new IdentityRole(role));

 if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => { });  
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();  
app.UseMiddleware<LoggingMiddleare>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
