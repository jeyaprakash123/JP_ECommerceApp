using ECommerceApp.Auth.Api.Domain.Interface;
using ECommerceApp.Auth.Api.Domain.Models;
using ECommerceApp.Auth.Api.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Auth.Api.Infrastructure.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Login> FindUserByNameAsyc(string userName)
        {
            return await _dataContext.Logins.Include(u=>u.Roles).SingleOrDefaultAsync(x=>x.Username == userName);
        }

        public async Task<Login> CreateUserAsync(Login login)
        {
            await _dataContext.Logins.AddAsync(login);
            await _dataContext.SaveChangesAsync();
            return login;
        }

        public async Task<Login> UpdateUserAsync(Login login)
        {
            _dataContext.Logins.Update(login);
            await _dataContext.SaveChangesAsync();
            return login;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var res = await _dataContext.Logins.FirstOrDefaultAsync(x=>x.Username == username);

             _dataContext.Logins.Remove(res);
            if (res is null) return false;
            await _dataContext.SaveChangesAsync();
            return true;
        
        }
    }
}
