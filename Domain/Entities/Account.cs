using System;
using System.Collections.Generic;
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
    public class Account
    {
        public int AccountId {  get; set; }
        public AccountStatus Status { get; set; }
        [ForeignKey("User")]
        public string Id { get; set; }
        public virtual User User { get; set; }
    }

    }

