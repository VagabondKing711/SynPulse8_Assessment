using SynPulse8_Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynPulse8_DataAccess.Interfaces
{
    public interface IUserProvider
    {
        Task<string> GetPasswordHashAsync(string id);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetUsersAsync();
    }
}
