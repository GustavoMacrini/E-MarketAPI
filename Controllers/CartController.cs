using E_Market.Server.Domain.CartItems;
using E_Market.Server.Services.Carts;
using Microsoft.AspNetCore.Mvc;

namespace E_Market.Server.Controllers
{
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {

        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("/CartItem")]
        public async Task<IResult> CreateCartItemAsync(CartItemRequest request)
        {
            try
            {
                await _cartService.createCartItem(request);
                return Results.Created();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }
    }
}
