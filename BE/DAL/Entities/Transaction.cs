using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid UserId { get; set; } 
        public Guid OrderId { get; set; } 
        public decimal Amount { get; set; } 
        public string TransactionType { get; set; } 
        public string Status { get; set; } 
        public DateTime TransactionDate { get; set; } 
        public string PaymentMethod { get; set; } 
        public string TransactionReference { get; set; } 

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Order Order { get; set; }
    }
}
