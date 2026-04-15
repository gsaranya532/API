using WebAPI.Model;

namespace WebAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        // new method for JWT login
        Task<User> GetByUsernameAsync(string name);
        Task<int> CountUsersAsync();

    }
}

