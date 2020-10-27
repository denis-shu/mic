using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repos.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetPRoducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetproductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
        Task Create(Product pr);
        Task<bool> Update(Product p);
        Task<bool> Delete(string id);
    }
}
