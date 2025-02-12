using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; } 
        public decimal TotalAmount { get; set; } 
        public string OrderStatus { get; set; } 
        public DateTime OrderDate { get; set; } 
        public string PaymentStatus { get; set; }
 
        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
