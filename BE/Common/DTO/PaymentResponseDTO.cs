namespace Common.DTO.Payment
{
    public class PaymentResponseDTO
    {
        public Guid UserId { get; set; }
        public string TransactionInfo { get; set; } = null!;
        public string TransactionNumber { get; set; } = null!;
        public bool IsSuccess { get; set; }

        // Thêm thông tin voucher
        public string? VoucherCode { get; set; }       // Mã voucher (nếu có)
        public decimal DiscountAmount { get; set; }    // Số tiền giảm từ voucher
        public double DiscountPercentage { get; set; } // Phần trăm giảm giá từ voucher
        public string? VoucherStatus { get; set; }     // Trạng thái voucher (Valid, Exp
    }
}

