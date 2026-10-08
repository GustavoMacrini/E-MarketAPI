using E_Market.Server.Domain.CartItems;
using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Categories;
using E_Market.Server.Domain.Products;
using E_Market.Server.Domain.Users;
using E_Market.Server.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Market.Server.Services.Users
{
    public class UserService
    {
        private ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        private UserResponse _buildUserResponse(User user)
        {
            CartResponse cartResponse = new CartResponse(
                  user.Cart.Id,
                  user.Cart.CartItems.Select(
                      c => new CartItemResponse(
                              c.Id,
                              new ProductResponse(
                                  c.Product.Id,
                                  c.Product.Name,
                                  new CategoryResponse(
                                      c.Product.Category.Id,
                                      c.Product.Category.Name
                                  ),
                                  c.Product.Description,
                                  c.Product.Price
                              ),
                              c.Quantity
                              
                          )
                      ).ToList()
                  );

            return new UserResponse(user.Id, user.Name, user.Email, user.Orders, cartResponse);
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest request)
        {
            try
            {
               Cart cart = new Cart(
                   new List<CartItem>(),
                   "CreatedBy"
               );

                await _context.Carts.AddAsync(cart);

                User user = new User(
                    request.Name,
                    request.Email,
                    request.Password,
                    null,
                    cart
                    );
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return _buildUserResponse(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            try
            {
                List<User> users = await _context.Users
                                    .Include(p => p.Orders)
                                    .Include(p => p.Cart)
                                        .ThenInclude(c => c.CartItems)
                                        .ThenInclude(ci => ci.Product)
                                        .ThenInclude(p => p.Category)
                                    .ToListAsync();

                List<UserResponse> response = users.Select(
                    u => _buildUserResponse(u)        
                ).ToList();
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> GetUserAsync(Guid id)
        {
            try
            {
                User user = await _context.Users.Where(u => u.Id == id)
                     .Include(p => p.Cart)
                        .ThenInclude(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                    .Include(u => u.Orders)
                    .FirstOrDefaultAsync();
                if(user == null)
                {
                    throw new Exception($"User id {id} not found.");
                }

                return _buildUserResponse(user);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
