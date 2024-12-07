using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO?> GetUserByLoginAsync(string? login);
        Task<UserDTO?> GetUserByTokenAsync(string? token);
        Task<UserDTO?> Register(UserDTO newUser);
    }
}
