using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ticket
{
    public class TicketDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter the Name of Event.")]
        public string Name { get; set; }
        public double Price { get; set; }
        [Display(Name = "Start  Date of Event")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End  Date of Event")]
        [DataType(DataType.Date)]
      // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please enter the Start Date of Event.")]
        [Display(Name = "Start  Date of Event")]
        [DataType(DataType.Time)]
       // [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Please enter the End Time of Event.")]
        [Display(Name = "End Time  of Event")]
        [DataType(DataType.Time)]
      //  [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan  EndTime { get; set; }
        public Guid EventId { get; set; }
        public string Location { get; set; }
        public string TicketColor { get; set; }

    }
}
