
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.DTOs;
using Application.Interfaces;

namespace BookStore.Controllers
{
 
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly IConfiguration _configuration;
        private readonly IUserService _userService; 

        public AuthController(
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager,
                IConfiguration configuration , IUserService userService)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _configuration = configuration;
            _userService = userService;
        }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] UserDTOs dto)
            {

            await _userService.RegisterAsync(dto);
            return Ok("User registered");
        }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginDto dto)
            {
            var token = await _userService.LoginAsync(dto);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }

        [Authorize(Policy = "UserOrAdmin")]
        [HttpPost("logout")]
            public IActionResult Logout()
            {
                // With JWT, client discards token
                return Ok(new { message = "Logged out" });
            }
        }
    }

