using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Stripe
{
   public  interface IStripeService
    {
        Task<string> CreateSubscription(string customerId, string priceId);
    }
}
