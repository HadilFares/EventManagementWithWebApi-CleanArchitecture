﻿using Application.Dtos.Account;
using Application.Interfaces.UserRepository;
using Domain.Entities;
using Infrastructure.Context;
using MailKit.BounceMail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly EventlyDbContext _context;
        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] SignUp model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model data");
            }
            var origin = Request.Headers["origin"];
            var result= await _userService.AddUser(model, origin);

            return Ok(result);
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
        [HttpGet("AllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var organizers = await _userManager.GetUsersInRoleAsync("Organizer");
            var participants = await _userManager.GetUsersInRoleAsync("Participant");

            // Concatenate the lists of users with these roles
            var usersWithRoles = organizers.Concat(participants);
            var users = _userManager.Users;
            var userDTOs = usersWithRoles.Select(user => new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName,
                Number = user.PhoneNumber,
                Password = user.PasswordHash
                // Map other properties as needed
            }).ToList();

            return Ok(userDTOs);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            
           




            return Ok(new { message = "Logout successful" });
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
        [Authorize(Roles = "Admin")]
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

