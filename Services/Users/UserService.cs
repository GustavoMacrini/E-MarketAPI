using E_Market.Server.Domain.Auth;
using E_Market.Server.Domain.CartItems;
using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Categories;
using E_Market.Server.Domain.Products;
using E_Market.Server.Domain.Users;
using E_Market.Server.Services.Auth;
using E_Market.Server.Services.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Market.Server.Services.Users
{
    public class UserService
    {
        private ApplicationDbContext _context;
        private IJwtService _jwtService;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context, IJwtService jwtService, UserManager<User> userManager)
        {
            _context = context;
            _jwtService = jwtService;
            _userManager = userManager;
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
            User? existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email já cadastrado.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            Guid userId = Guid.NewGuid();
            Cart cart = new Cart(new List<CartItem>(), userId);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            User user = new User(request.Name, request.Email, [], cart)
            {
                Id = userId,
            };

            IdentityResult createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                await transaction.RollbackAsync();
                string errors = string.Join(" ", createResult.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            await transaction.CommitAsync();

            return _buildUserResponse(user);
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

        public async Task<LoginResponse> LoginAsync(UserLoginRequest request)
        {
            User? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginResponse("Email or Password is incorrect.", "");
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return new LoginResponse("Email or Password is incorrect.", "");
            }

            string token = _jwtService.GenerateToken(user);
            return new LoginResponse("", token);
        }
    }
}
