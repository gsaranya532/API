using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Model;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        
        public UserController(IUserRepository repository) => _repository = repository;
        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var count = await _repository.CountUsersAsync();

                return Ok(new
                {
                    message = "EF Core is connected successfully!",
                    userCount = count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Database connection failed!",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var u = await _repository.GetByIdAsync(id);
            return u == null ? NotFound() : Ok(u);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            await _repository.AddAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Role = user.Role;
            await _repository.UpdateAsync(existing);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

       

    }
}
