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
        public decimal TotalPrice { get; set; }

        public string? VoucherCode { get; set; } // Thêm thuộc tính này





    }
}
