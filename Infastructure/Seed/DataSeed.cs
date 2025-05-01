using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Seed
{
    public static class DataSeed
    {
        public static async Task SeedDataAsync(this IHost app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<IConfiguration>();
            var userService = services.GetRequiredService<IUserService>();

            var adminUser = config["AdminUser:UserName"];
            var adminPass = config["AdminUser:Password"];
            await userService.SeedAdminAsync(adminUser, adminPass);
        }
    }
}
