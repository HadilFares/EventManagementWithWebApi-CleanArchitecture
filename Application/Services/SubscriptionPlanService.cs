using Application.Interfaces.SubscriptionPlanRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SubscriptionPlanService : BaseRepository<SubscriptionPlan>, ISubscriptionPlanService
    {

        private readonly EventlyDbContext _context;
        public SubscriptionPlanService(EventlyDbContext context) : base(context)
        {
            _context = context;

        }

    }
}
