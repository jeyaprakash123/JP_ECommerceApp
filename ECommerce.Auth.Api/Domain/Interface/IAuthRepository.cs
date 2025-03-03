using ECommerceApp.Auth.Api.Domain.Models;

namespace ECommerceApp.Auth.Api.Domain.Interface
{
    public interface IAuthRepository
    {
        Task<Login> FindUserByNameAsyc(string username);
        Task<Login> CreateUserAsync(Login login);

        Task<Login> UpdateUserAsync(Login login);

        Task<bool> DeleteUserAsync(string username);
    }
}
