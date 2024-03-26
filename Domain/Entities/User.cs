using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Stripe;

namespace Domain.Entities
{
    [Table("User")]
    public class User :IdentityUser
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        // public Guid? AccountId { get; set; }
        public  Subscription? Subscription { get; set; }
        public  Account? Account { get; set; }
        public  ICollection<Category>? Categories { get; set; }
        public ICollection<Event>? Events { get; set; }
        public ICollection<Comment>? Comments { get; set; }



    }
}
