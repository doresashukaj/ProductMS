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
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserForLoginDto userForLogin)

        {
            if (userForLogin is null)
                return BadRequest(new { Errors = new[] { "Invalid request." } });

            var user = await _userManager.FindByEmailAsync(userForLogin.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userForLogin.Password))
            {
                return Unauthorized(new { Errors = new[] { "Invalid email or password." } });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);

            return Ok(new { Token = token });
        }




        /* [HttpPost("assign-role")]
         [Authorize(Roles = "Admin")] 
         public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto assignRoleDto)
         {
             if (assignRoleDto == null)
             {
                 return BadRequest("Invalid data.");
             }

             var user = await _userManager.FindByEmailAsync(assignRoleDto.Email);
             if (user == null)
             {
                 return NotFound("User not found.");
             }


             var roleExist = await _roleManager.RoleExistsAsync(assignRoleDto.Role);
             if (!roleExist)
             {
                 return BadRequest("Role does not exist.");
             }


             var result = await _userManager.AddToRoleAsync(user, assignRoleDto.Role);
             if (!result.Succeeded)
             {
                 return BadRequest(result.Errors);
             }

             return Ok(new { Message = "Role assigned successfully." });
         }*/



        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto updateUserDto)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();  
            }

            
            if (currentUser.Id != User.FindFirst(c => c.Type == "Id")?.Value)
            {
                return Forbid();  
            }

            
            currentUser.FirstName = updateUserDto.FirstName ?? currentUser.FirstName;
            currentUser.LastName = updateUserDto.LastName ?? currentUser.LastName;
            currentUser.PhoneNumber = updateUserDto.PhoneNumber ?? currentUser.PhoneNumber;
            currentUser.Email = updateUserDto.Email ?? currentUser.Email;
            currentUser.DateOfBirth = updateUserDto.DateOfBirth ?? currentUser.DateOfBirth;
            currentUser.Gender = updateUserDto.Gender ?? currentUser.Gender;

            var result = await _userManager.UpdateAsync(currentUser);

            if (result.Succeeded)
            {
                return Ok(currentUser);  
            }
            else
            {
                return BadRequest(result.Errors);  
            }
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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Unauthorized();

            return Ok(new { Message = "Logout successful." });
        }



    }
}
