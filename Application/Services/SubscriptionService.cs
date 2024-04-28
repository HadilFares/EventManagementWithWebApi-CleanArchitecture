using Application.Interfaces.SubscriptionRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SubscriptionService : BaseRepository<Subscription>, ISubscriptionService
    {

        private readonly EventlyDbContext _context;



        public SubscriptionService(EventlyDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<Subscription> GetSubscriptionByPlan(Guid PlanId)
        {
            return await _context.Subscriptions
                 .Include(s => s.SubscriptionPlan)
                 .FirstOrDefaultAsync(s => s.PlanId == PlanId);
        }

        public async Task<List<Subscription>> GetSubscriptionsByType(SubscriptionType subscriptionType)
        {
            return await _context.Subscriptions
                .Include(s => s.SubscriptionPlan) // Include the related plan
                .Where(s => s.SubscriptionPlan.Type == subscriptionType)
                .ToListAsync();
        }
    }
}
