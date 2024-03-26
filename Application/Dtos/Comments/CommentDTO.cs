using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Comments
{
    public class CommentDTO
    {

        [Required]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; } = 0;

    }
}
