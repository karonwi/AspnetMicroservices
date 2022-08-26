using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public Task CreateProduct(Product product)
        {
            return _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            DeleteResult deleteResult = await _catalogContext.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0; 
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);
            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<Product> GetProductById(string productId)
        {
            return await _catalogContext.Products.Find(x => x.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, productName);
            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext.Products.Find(p => true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _catalogContext.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);
            //var tryingToUpdateResult = await _catalogContext.Products.UpdateOneAsync(filter: x => x.Id,product.Id, update:  product => product, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
