using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; } 
        public Guid CourseId { get; set; } 
        public decimal TotalPrice { get; set; } 

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Course Course { get; set; }
    }
}
