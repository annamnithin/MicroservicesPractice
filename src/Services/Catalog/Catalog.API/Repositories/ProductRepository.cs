﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }
        public async Task CreateProductAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult result =  await _context
                        .Products
                        .DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;

        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _context
                           .Products
                           .Find(filter)
                           .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context
                        .Products
                        .Find(p => p.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context
                        .Products
                        .Find(filterDefinition)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context
                    .Products
                    .Find(p => true)
                    .ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updateResult = await _context
                                     .Products
                                     .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged 
                    && updateResult.ModifiedCount > 0;
        }
    }
}
