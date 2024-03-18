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

    public enum AccountStatus
    {
        Active,   
        Pending,  
        Canceled    
    }
    public  class Account : BaseEntity
    {
        
        public AccountStatus Status { get; set; }
        public string? UserId { get; set; }

        public User? User { get; set; }
    }

    }

