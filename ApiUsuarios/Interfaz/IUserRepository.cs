using ApiUsuarios.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiUsuarios.Interfaz
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<UserModel> GetUserByIdAsync(string id);
        Task CreateUserAsync(UserModel user);
        Task<bool> UpdateUserAsync(string id, UserModel user);
        Task<bool> DeleteUserAsync(string id);
    }
}
