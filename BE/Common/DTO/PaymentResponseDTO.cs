namespace Common.DTO.Payment
{
    public class PaymentResponseDTO
    {
        public Guid UserId { get; set; }
        public string TransactionInfo { get; set; } = null!;
        public string TransactionNumber { get; set; } = null!;
        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }  // Mã trạng thái HTTP (200, 400, 500)
        public string Message { get; set; } = string.Empty; // Thông báo kết quả
        public object? Result { get; set; } // Dữ liệu kết quả (URL thanh toán, transaction, ...)


    }
}

