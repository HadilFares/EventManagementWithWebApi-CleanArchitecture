using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.SubscriptionRepository
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetSubscriptionByPlan(Guid PlanId);
        Task<List<Subscription>> GetSubscriptionsByType(SubscriptionType subscriptionType);


    }
}
