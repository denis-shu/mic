using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repos.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repos
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatologContext _context;

        public ProductRepository(ICatologContext context)
        {
            _context = context;
        }

        public async Task Create(Product pr)
        {
            await _context.Products.InsertOneAsync(pr);
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(pr => pr.Id, id);

            var res = await _context.Products.DeleteOneAsync(filter);

            return res.IsAcknowledged && res.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Category, category);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetproductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetPRoducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<bool> Update(Product p)
        {
            var updateRes = await _context
                .Products
                .ReplaceOneAsync(filter: g => g.Id == p.Id, replacement: p);

            return updateRes.IsAcknowledged && updateRes.ModifiedCount > 0;
        }
    }
}
