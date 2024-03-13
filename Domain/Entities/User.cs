using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User :IdentityUser
    {
        [Key]
        public override string Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual  Account account { get; set; }
       

    }
}
