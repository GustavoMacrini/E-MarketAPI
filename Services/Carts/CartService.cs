using E_Market.Server.Domain.CartItems;
using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Categories;
using E_Market.Server.Domain.Products;
using E_Market.Server.Services.Data;
using Microsoft.EntityFrameworkCore;


namespace E_Market.Server.Services.Carts
{
    public class CartService
    {
        private ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
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
                //ToDo: CartItemRequest não mandar qual cart é, mas sim pegar o cart do usuário logado. Para isso, precisa de um UserId no request.
                string msgErro = null;
                Cart cart = await _context.Carts
                    .Include(c => c.CartItems)                   
                    .Where(c => c.Id == request.CartId)
                    .FirstOrDefaultAsync();

                Product product = await _context.Products
                                  .Where(p => p.Id == request.ProductId)
                                  .FirstOrDefaultAsync();

                if (cart == null)
                {
                    msgErro = $"Cart id {request.CartId} not found.";
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
