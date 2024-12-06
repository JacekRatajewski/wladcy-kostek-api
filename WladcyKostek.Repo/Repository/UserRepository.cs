using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Repo.Context;
using WladcyKostek.Repo.Entities;

namespace WladcyKostek.Repo.Repository
{
    public class UserRepository : IUserRepository
    {
        private DatabaseContext _database;

        public UserRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<UserDTO?> GetUserByLoginAsync(string? login)
        {
            var user = await _database.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user is not null)
            {
                return new UserDTO
                {
                    Login = user.Login,
                    Email = user.Email,
                    FromGoogle = user.FromGoogle,
                    AccountCreationDate = user.AccountCreationDate,
                };
            }
            return null;
        }

        public async Task<UserDTO?> GetUserByTokenAsync(string? token)
        {
            var user = await _database.Users.FirstOrDefaultAsync(x => x.Token == token);
            if (user is not null)
            {
                return new UserDTO
                {
                    Login = user.Login,
                    Email = user.Email,
                    FromGoogle = user.FromGoogle,
                    AccountCreationDate = user.AccountCreationDate,
                };
            }
            return null;
        }

        public async Task<UserDTO?> Register(UserDTO newUser)
        {
            var userId = await _database.Users.AddAsync(new User
            {
                Password = newUser.Password,
                AccountCreationDate = DateTime.UtcNow,
                Email = newUser.Email,
                FromGoogle = newUser.FromGoogle,
                Login = newUser.Login
            });
            var added = await _database.SaveChangesAsync();
            if (added > 0)
            {
                var user = await _database.Users.FirstOrDefaultAsync(x => x.Id == userId.Entity.Id);
                if (user is not null)
                {
                    return new UserDTO
                    {
                        Login = user.Login,
                        Email = user.Email,
                        FromGoogle = user.FromGoogle,
                        AccountCreationDate = user.AccountCreationDate,
                    };
                }
                return null;
            };
            return null;
        }
    }
}
