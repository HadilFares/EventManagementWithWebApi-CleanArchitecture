using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Stripe;

namespace Domain.Entities
{
   public  class Subscription:BaseEntity
    {
        [Required]
        public Guid? PlanId { get; set; }

        public SubscriptionPlan? SubscriptionPlan { get; set; }

        [Required]
        public DateTime SubscriptionStartDate { get; set; }

        [Required]
        public DateTime SubscriptionEndDate { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}
