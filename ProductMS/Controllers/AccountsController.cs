using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductMS.DTO;
using ProductMS.Entities;
using ProductMS.JwTFeatures;
using System.Security.Claims;

namespace ProductMS.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;



        public AccountsController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler)
        {
            
            _mapper = mapper;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration is null)
                return BadRequest();

            var existingUser = await _userManager.FindByEmailAsync(userForRegistration.Email);
            if (existingUser != null)
                return BadRequest(new { Errors = new[] { "Email is already in use." } });

            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });

            }
            await _userManager.AddToRoleAsync(user, "User");
            return StatusCode(201);


        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByNameAsync(userForAuthentication.Email!);
                if (user is null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password!))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication"});

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);
            return Ok(new AuthResponseDto { IAuthSuccessful = true, Token = token });
                }
       

    }
}
