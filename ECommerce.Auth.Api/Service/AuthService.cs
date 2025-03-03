using ECommerceApp.Auth.Api.Domain.Interface;
using ECommerceApp.Auth.Api.Domain.Models;

namespace ECommerceApp.Auth.Api.Service
{
    public class AuthService : IAuthService
    {
        public IAuthRepository _authRepository { get; set; }
        public AuthService(IAuthRepository AuthRepository)
        {
            _authRepository = AuthRepository;
        }

        public async Task<Login> FindByNameAsync(string name)
        {
            return await _authRepository.FindUserByNameAsyc(name);
        }

        public async Task<Login> CreateUserAsync(Login login)
        {

            login.UserId = Guid.NewGuid().ToString().Substring(0, 4);
            login.Password = BCrypt.Net.BCrypt.HashPassword(login.Password);
            login.RoleId = 2;

            return await _authRepository.CreateUserAsync(login);

        }
        public async Task<Login> UpdateUserAsync(string username, string password)
        {
            var updatedLogin = await _authRepository.FindUserByNameAsyc(username);
            if (updatedLogin is null) return null;

            updatedLogin.Password = BCrypt.Net.BCrypt.HashPassword(password);
            var res = await _authRepository.UpdateUserAsync(updatedLogin);
            return res;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            return await _authRepository.DeleteUserAsync(username);
        }
    }
}
