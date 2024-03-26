using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? OrganizerId { get; set; }
        public  virtual User? User { get; set; }
        public ICollection<Event>? Events { get; set; }

    }
}
