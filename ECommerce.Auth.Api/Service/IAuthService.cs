using ECommerceApp.Auth.Api.Domain.Models;

namespace ECommerceApp.Auth.Api.Service
{
    public interface IAuthService
    {
        Task<Login> FindByNameAsync(string name);

        Task<Login> CreateUserAsync(Login login);

        Task<Login> UpdateUserAsync(string username, string password);

        Task<bool> DeleteUserAsync(string username);
    }
}
