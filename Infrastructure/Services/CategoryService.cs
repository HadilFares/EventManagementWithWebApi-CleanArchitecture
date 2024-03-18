using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
    internal class CategoryService : BaseRepository<Category>
    {
        public CategoryService(EventlyDbContext context) : base(context)
        {

        }
    }
}
