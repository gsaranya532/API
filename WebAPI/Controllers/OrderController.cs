using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        public OrderController(IOrderRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var o = await _repository.GetByIdAsync(id);
            return o == null ? NotFound() : Ok(o);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            order.Id = 0;
            await _repository.AddAsync(order);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.ProductName = order.ProductName;
            existing.Quantity = order.Quantity;
            existing.OrderDate = order.OrderDate;
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
