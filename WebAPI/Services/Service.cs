using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;
using WebAPI.Repositories;


namespace WebAPI.Services
{
    public class Service
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _appdbContext;
        public Service(IUserRepository userRepository,AppDbContext appDbContext)
        {
            _userRepository = userRepository;
            _appdbContext= appDbContext;

        }

        // One-time migration method
        public async Task HashExistingPasswordsAsync()
        {
            if (_appdbContext.Users == null)
            {
                return;
            }

            var users = await _appdbContext.Users.ToListAsync();

            foreach (var user in users)
            {
                if (!user.PasswordHash.StartsWith("$2a$")) // Only hash if not already hashed
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                }
            }

            await _appdbContext.SaveChangesAsync();
        }
        // ✅ Register user with hashed password
        public async Task<User> RegisterUserAsync(string username, string password, string role)
        {
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Name = username,
                PasswordHash = hashedPassword,
                Role = role,
                Email = string.Empty // Set required Email property to avoid CS9035
            };

            // Save to DB
            await _userRepository.AddAsync(user);

            return user;
        }
        // ✅ Get user by username (for login)
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

       
    }
}