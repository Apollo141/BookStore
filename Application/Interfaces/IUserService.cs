using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(UserDTOs request);
        Task<string> LoginAsync(LoginDto request);
        Task LogoutAsync();
        Task SeedAdminAsync(string userName, string password);
    }
}
