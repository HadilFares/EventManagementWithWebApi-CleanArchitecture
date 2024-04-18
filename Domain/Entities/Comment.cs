using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment:BaseEntity
    {
        [Required]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; } = 0;
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public Guid? EventId { get; set; }
        public virtual Event
            ? Event { get; set; }


    }
}
