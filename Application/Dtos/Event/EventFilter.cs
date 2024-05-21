using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Event
{
   public  class EventFilter
    {

        public string? CategoryName { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
    }
}
