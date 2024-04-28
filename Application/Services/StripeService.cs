using Application.Interfaces.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StripeService : IStripeService
    {

        public StripeService() { }

        public Task<string> CreateSubscription(string customerId, string priceId)
        {
            throw new NotImplementedException();
        }
    }
}
