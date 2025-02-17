using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class PaymentRequestDTO
    {
        public Guid UserId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public double TotalPrice { get; set; }
    }
}
