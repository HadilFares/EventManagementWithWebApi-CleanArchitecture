﻿using Application.Dtos.Accounts;
using Application.Dtos.Statistics;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UserRepository
{
    public interface IUserService
    {
        Task<List<string>> GetUserRolesAsync(User user);
        Task<AuthResponse> AddUser(SignUp model, string orgin);
        Task<List<UserRole>> GetRoleCounts();

    }
}
