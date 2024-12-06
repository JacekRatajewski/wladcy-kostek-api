using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO?> GetUserByLoginAsync(string? login);
        Task<UserDTO?> GetUserByTokenAsync(string? token);
        Task<bool> Register(UserDTO newUser);
    }
}
