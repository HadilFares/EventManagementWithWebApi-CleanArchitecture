using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Event
{
    public class EventDTO
    {
        [Required(ErrorMessage = "Please enter the Name of Event.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the Description of Event.")]
        [MaxLength(150)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter the Date of Event.")]
        [Display(Name = "Date of Event")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Please select the Type of Event.")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please enter the Location of Event.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Please enter the Price of Event.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Please enter the NbStand of Event.")]
        public int NbStand { get; set; }
        public int Ratings { get; set; }
        [Required]
        public string OrganizerId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        
    }
}
