using Domain.Entities;
using Microsoft.AspNetCore.Http;
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

        public IFormFile? Photo {  get; set; }
        public string? OrganizerName { get; set; }
        public string? OrganizerLastName { get; set; }
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Please enter the Name of Event.")]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the Description of Event.")]
        [MaxLength(500)]
        public string Description { get; set; }
      
        [Required(ErrorMessage = "Please enter the Start  Date of Event.")]
        [Display(Name = " Start Date of Event")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the End Date of Event.")]
        [Display(Name = "End  Date of Event")]
        [DataType(DataType.Date)]
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
        [Required(ErrorMessage = "Please select the Type of Event.")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please enter the Location of Event.")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Please enter the Price of Event.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Please enter the NbStand of Event.")]
        public int NbStand { get; set; }
        public int? Ratings { get; set; }
        [Required]
        public string OrganizerId { get; set; }
        [Required(ErrorMessage = "Please enter the category of Event.")]
        public string CategoryName { get; set; }
        

    }
}
