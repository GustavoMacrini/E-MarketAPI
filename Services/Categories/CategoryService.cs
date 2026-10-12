using E_Market.Server.Domain.Categories;
using E_Market.Server.Services.Auth;
using E_Market.Server.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Market.Server.Services.Categories
{
    public class CategoryService
    {
        private ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request)
        {

            Boolean categoryExists = await _context.Categories.AnyAsync(category => category.Name == request.Name);

            if (categoryExists)
            {
                throw new Exception($"Category {request.Name} Already Exists.");
            }

            Guid userId = _httpContextAccessor.HttpContext!.User.GetUserId();
            Category category = new Category(request.Name, userId, userId);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new CategoryResponse(category.Id, category.Name);
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            List<CategoryResponse> response = categories.Select(c => new CategoryResponse(c.Id, c.Name)).ToList(); 
            return response;
        }

        public async Task<CategoryResponse> GetCategoryAsync(Guid id)
        {
            Category category = await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null) 
            {
                throw new Exception($"Category {id} not found.");
            }

            return new CategoryResponse(category.Id, category.Name);
        }

        public async Task DeleteCategory(Guid id)
        {
            Category category = await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();

            if(category == null)
            {
                throw new Exception($"Category {id} not found.");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }



    }
}
