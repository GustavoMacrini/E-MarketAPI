using E_Market.Server.Domain.Users;
using E_Market.Server.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace E_Market.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;
        }

        [HttpPost()]
        public async Task<IResult> CreateUserAsync(UserRequest request)
        {
            await _userService.CreateUserAsync(request);
            return Results.Created();
        }

        [HttpGet()]
        public async Task<IResult> GetAllUsersAsync()
        {
            try
            {
                List<UserResponse> response = await _userService.GetAllUsersAsync();
                return Results.Ok(response);
            }
            catch(Exception e)
            {
                return Results.StatusCode(500);
            }

        }

        [HttpGet("{id:guid}")]
        public async Task<IResult> GetUsersAsync([FromRoute] Guid id)
        {
            try
            {
                UserResponse response = await _userService.GetUserAsync(id);
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                return Results.StatusCode(500);
            }

        }
    }
}
