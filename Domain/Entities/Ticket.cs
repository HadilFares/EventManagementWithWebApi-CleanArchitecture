using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
  
        public class Ticket : BaseEntity
        {
            [Required]
            public string Name { get; set; }
            public virtual Event Event { get; set; }
            public Guid EventId { get; set; }
            public string Ticketcolor { get; set; }
            public double Price { get; set; }
            [Display(Name = "Start  Date of Event")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; set; }
            [Display(Name = "End  Date of Event")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime EndDate { get; set; }

            [Required(ErrorMessage = "Please enter the Start Date of Event.")]
            [Display(Name = "Start  Date of Event")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
            public TimeSpan StartTime { get; set; }
            [Required(ErrorMessage = "Please enter the End Time of Event.")]
            [Display(Name = "End Time  of Event")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
            public TimeSpan? EndTime { get; set; }
            public string Location { get; set; }
        }
    }
