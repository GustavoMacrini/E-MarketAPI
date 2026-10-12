using E_Market.Server.Domain.CartItems;
using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Categories;
using E_Market.Server.Domain.Products;
using E_Market.Server.Domain.Users;
using E_Market.Server.Services.Auth;
using E_Market.Server.Services.Data;
using Microsoft.EntityFrameworkCore;


namespace E_Market.Server.Services.Carts
{
    public class CartService
    {
        private ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private CartItemResponse _buildCartItemResponse(CartItem cartItem, Guid cartId)
        {
            CartItemResponse cartItemResponse = new CartItemResponse(
                   cartItem.Id,
                   new ProductResponse(
                       cartItem.Product.Id,
                       cartItem.Product.Name,
                       new CategoryResponse(
                           cartItem.Product.Category.Id,
                           cartItem.Product.Category.Name
                       
                       ),
                       cartItem.Product.Description,
                       cartItem.Product.Price
                    ),
                   cartItem.Quantity                   
            );
            return cartItemResponse;
        }

        public async Task<CartItemResponse> createCartItem(CartItemRequest request)
        {
            try
            {
                Guid userId = _httpContextAccessor.HttpContext!.User.GetUserId();
                User? user = await _context.Users
                    .Include(u => u.Cart)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user?.Cart == null)
                {
                    throw new Exception("User or Cart not found.");
                }

                string? msgErro = null;
                Cart? cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.Id == user.Cart.Id);

                Product? product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == request.ProductId);

                if (cart == null)
                {
                    msgErro = $"Cart id {user.Cart.Id} not found.";
                }

                
                if (product == null)
                {
                    msgErro += msgErro != null ? "/n" : "";
                    msgErro += $"Product id {request.ProductId} not found.";
                }

                if(msgErro != null)
                {
                    throw new Exception(msgErro);
                }

                CartItem cartItem = new CartItem(product, request.Quantity);
                await _context.CartItems.AddAsync(cartItem);
                cart.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                return _buildCartItemResponse(cartItem, cart.Id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
