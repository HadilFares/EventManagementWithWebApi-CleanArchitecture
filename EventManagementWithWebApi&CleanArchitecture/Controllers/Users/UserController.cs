using Application.Dtos.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                var user = new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    UserName=userDTO.Username,
                    PhoneNumber=userDTO.Number,
                    PasswordHash=userDTO.Password
                    // Set other properties as needed
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO
            {
               // Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Number = user.PhoneNumber,
                Password = user.PasswordHash


                // Map other properties as needed
            };

            return Ok(userDTO);
        }


        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users;
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var userDTO = new UserDTO
                {
                   // Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName,
                    Number = user.PhoneNumber,
                    Password = user.PasswordHash
                    // Map other properties as needed
                };
                userDTOs.Add(userDTO);
            }

            return Ok(userDTOs);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDTO userDTO)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = userDTO.FirstName;
            existingUser.LastName = userDTO.LastName;
            existingUser.Email = userDTO.Email;
            existingUser.UserName = userDTO.Username;
            existingUser.PhoneNumber = userDTO.Number;
            existingUser.PasswordHash = userDTO.Password;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }

}

