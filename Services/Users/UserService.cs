using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Orders;
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

        public async Task<UserResponse> CreateUserAsync(UserRequest request)
        {
            try
            {
               Cart cart = new Cart(
                   new List<Product>(),
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
                return new UserResponse(user.Id, user.Name, user.Email, user.Orders, user.Cart);
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
                List<User> users = await _context.Users.Include(p => p.Orders)
                                                       .Include(p => p.Cart).ToListAsync();

                List<UserResponse> response = users.Select(
                    u => new UserResponse(
                        u.Id, 
                        u.Name, 
                        u.Email,
                        u.Orders,
                        u.Cart
                    )
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
                    .Include(u => u.Cart)
                    .Include(u => u.Orders)
                    .FirstOrDefaultAsync();
                if(user == null)
                {
                    throw new Exception($"User id {id} not found.");
                }

                UserResponse response = new UserResponse(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Orders,
                    user.Cart
                );

                return response;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
