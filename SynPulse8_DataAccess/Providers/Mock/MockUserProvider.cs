using SynPulse8_Abstractions;
using SynPulse8_DataAccess.Interfaces;
using static SynPulse8_Abstractions.User;

namespace SynPulse8_DataAccess.Providers.Mock
{
    public class MockUserProvider : IUserProvider
    {
        public Task<string> GetPasswordHashAsync(string id)
        {
            return Task.Run(() => {
                return "bWljaGFlbDpzeW5QdWxzZTg=";
            });
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return Task.Run(() => {
                return new User()
                {
                    EMailAddress = email,
                    Id = "123456",
                    CustomerId = "23456",
                    Status = AccountStatus.Active
                };
            });
        }

        public Task<User> GetUserByIdAsync(string id)
        {
            return Task.Run(() => {
                return new User()
                {
                    EMailAddress = "test@test.com",
                    Id = id,
                    CustomerId = "23456",
                    Status = AccountStatus.Active
                };
            });
        }

        public Task<List<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
