using WebAPI.Data;
using WebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            if (_context.Orders == null)
            {
                return Enumerable.Empty<Order>();
            }
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int id)
        {
            if (_context.Orders == null)
            {
                throw new InvalidOperationException("Orders DbSet is not initialized.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }
            return order;
        }
        public async Task AddAsync(Order order)
        {
            if (_context.Orders == null)
            {
                throw new InvalidOperationException("Orders DbSet is not initialized.");
            }
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Order order)
        {
            if (_context.Orders == null)
            {
                throw new InvalidOperationException("Orders DbSet is not initialized.");
            }
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            if (_context.Orders == null)
            {
                throw new InvalidOperationException("Orders DbSet is not initialized.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

    }
}
