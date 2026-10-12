using E_Market.Server.Domain.Auth;
using E_Market.Server.Domain.Users;
using E_Market.Server.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Market.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IResult> CreateUserAsync(UserRequest request)
        {
            try
            {
                UserResponse response = await _userService.CreateUserAsync(request);
                return Results.Created($"/api/v1/user/{response.Id}", response);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IResult> LoginAsync([FromBody] UserLoginRequest request)
        {          
            LoginResponse response = await _userService.LoginAsync(request);

            if (response.erro != "")
            {
                return Results.Json(new { error = response.erro }, statusCode: StatusCodes.Status401Unauthorized);
            }

            return Results.Ok(response);
            
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
