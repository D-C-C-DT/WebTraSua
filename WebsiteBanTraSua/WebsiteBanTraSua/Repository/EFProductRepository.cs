using Microsoft.EntityFrameworkCore;
using WebsiteBanTraSua.DataAccess;
using WebsiteBanTraSua.Models;

namespace WebsiteBanTraSua.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync(); // Include thông tin về category
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<Product> SearchProducts(string searchTerm)
        {
            // Truy vấn các sản phẩm có tên chứa searchTerm từ cơ sở dữ liệu
            return _context.Products.Where(p => p.Name.Contains(searchTerm)).ToList();
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
    }
}
