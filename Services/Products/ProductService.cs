using E_Market.Server.Domain.Categories;
using E_Market.Server.Domain.Products;
using E_Market.Server.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Market.Server.Services.Products
{
    public class ProductService
    {
        private ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
        {
            Category category = await _context.Categories.Where(c => c.Id == request.CategoryId).FirstOrDefaultAsync();
            if (category == null) 
            {
                throw new Exception($"Category {request.CategoryId} not found.");
            }

            Product product = new Product(request.Name, category, request.Description, request.Price, "CreatedBy");
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            CategoryResponse categoryResponse = new CategoryResponse(product.Category.Id, product.Category.Name);
            return new ProductResponse(product.Id, product.Name, categoryResponse, product.Description, product.Price);
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync(); 
            List<ProductResponse> response = products.Select(p => new ProductResponse(p.Id, p.Name, new CategoryResponse(p.Category.Id, p.Category.Name), p.Description, p.Price)).ToList();
            return response;
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid id)
        {
            Product product = await _context.Products.Include(pr => pr.Category).Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new Exception($"Product {id} not found.");
            }

            CategoryResponse categoryResponse = new CategoryResponse(product.Category.Id, product.Category.Name);
            return new ProductResponse(product.Id, product.Name, categoryResponse, product.Description, product.Price);
        }

        public async Task<ProductResponse> UpdateProductById(Guid id, ProductRequest request)
        {
            Product product = await _context.Products.Include(pr => pr.Category).Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new Exception($"Product {id} not found.");
            }

            Category category = await _context.Categories.Where(c => c.Id == request.CategoryId).FirstOrDefaultAsync();
            if (category == null)
            {
                throw new Exception($"Category {request.CategoryId} not found.");
            }

            product.EditValues(request.Name, category, request.Description, request.Price, "editedBy");
            await _context.SaveChangesAsync();

            CategoryResponse categoryResponse = new CategoryResponse(product.Category.Id, product.Category.Name);
            return new ProductResponse(product.Id, product.Name, categoryResponse, product.Description, product.Price);
        }



    }
}
