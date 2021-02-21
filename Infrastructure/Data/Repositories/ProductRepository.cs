using Core.Entities.ProductModels;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebshopDataContext _context;
        public ProductRepository(WebshopDataContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsByBrandAsync(int id)
        {
            return await _context.Products.FromSqlRaw($"select * from Products where ProductBrandId={id}").Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();                            
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductByNameAsync(string name)
        {            
           return await _context.Products.FromSqlRaw($"select * from Products where name = '{name}'").Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
