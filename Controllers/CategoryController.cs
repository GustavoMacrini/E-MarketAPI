
using E_Market.Server.Domain.Categories;
using E_Market.Server.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace E_Market.Server.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IResult> CreateCategory(CategoryRequest request)
        {
            try
            {
                CategoryResponse response = await _categoryService.CreateCategoryAsync(request);
                return Results.Created($"api/v1/category/{response.Id}", response);
            } catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IResult> GetCategoryById([FromRoute] Guid id)
        {
            if (id == null)
            {
                List<CategoryResponse> categories = await _categoryService.GetAllCategoriesAsync();
                return Results.Ok(categories);
            }

            try
            {
                CategoryResponse category = await _categoryService.GetCategoryAsync(id);
                return Results.Ok(category);
            }
            catch (Exception e)
            {
                return Results.NotFound(e.Message);
            }
        }

        [HttpGet()]
        public async Task<IResult> GetCategories()
        {
            List<CategoryResponse> categories = await _categoryService.GetAllCategoriesAsync();
            return Results.Ok(categories);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Results.Ok();
            }
            catch(Exception e)
            {
                return Results.NotFound(e.Message);
            }
        }


    }
}
