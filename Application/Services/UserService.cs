 using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;
using Application.DTOs;

namespace Application.Services
{
        public class UserService : IUserService
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IConfiguration _config;

            public UserService(
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration config)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _roleManager = roleManager;
                _config = config;
            }

            public async Task RegisterAsync(UserDTOs request)
            {
                var user = new IdentityUser { UserName = request.UserName, Email = request.Email };
                await _userManager.CreateAsync(user, request.Password);

                // assign User role
                await _userManager.AddToRoleAsync(user, "User");
            }

            public async Task<string> LoginAsync(LoginDto request)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null) return null;

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded) return null;

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var jwt = _config.GetSection("JWT");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwt["Issuer"],
                    audience: jwt["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiryMinutes"])),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            public async Task LogoutAsync()
            {
                // With JWT, logout is client-side: nothing to revoke unless using refresh tokens
                await Task.CompletedTask;
            }

            public async Task SeedAdminAsync(string userName, string password)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                var admin = await _userManager.FindByNameAsync(userName);
                if (admin == null)
                {
                    admin = new IdentityUser { UserName = userName, Email = userName, EmailConfirmed = true };
                    await _userManager.CreateAsync(admin, password);
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }

 
    }
    
}
