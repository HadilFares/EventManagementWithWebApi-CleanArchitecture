﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Context
{
    public class IdentityDbContext: IdentityDbContext<User>
    {
      /*  public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
        {
        }*/
    }
}
