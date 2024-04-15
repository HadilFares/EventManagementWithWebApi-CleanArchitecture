using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event:BaseEntity

    {
        [Required(ErrorMessage = "Please enter the Name of Event.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the Description of Event.")]
        [MaxLength(150)]
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
        public TimeSpan StartTime  { get; set; }
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
        public int NbStand {  get; set; }
        public int? Ratings { get; set; }
        public bool IsValidated { get; set; } = false;
        public string? UserId { get; set; }
        public  virtual User? User { get; set; }
        public Guid? CategoryId { get; set; }
        public  virtual Category? Category { get; set; }
        public ICollection<Comment>? Comments { get; set; }

    }
}
