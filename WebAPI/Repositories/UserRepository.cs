using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() 
            => await (_context.Users ?? throw new InvalidOperationException("Users DbSet is null.")).ToListAsync();
        public async Task<User> GetByIdAsync(int id)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            return user;
        }
        public async Task AddAsync(User user)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is null.");
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is null.");
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User> GetByUsernameAsync(string name)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is null.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with name '{name}' not found.");
            }
            return user;
        }
       
        public async Task<int> CountUsersAsync()
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is null.");
            }
            return await _context.Users.CountAsync();
        }

    }
}
