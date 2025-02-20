namespace Common.DTO
{
    public class VoucherDTO
    {
        public string VoucherCode { get; set; }  // Mã voucher
        public string Description { get; set; }  // Mô tả
        public int? DiscountPercentage { get; set; }  // Phần trăm giảm giá
        public bool IsActive { get; set; }  // Trạng thái hoạt động
        public DateTime StartDate { get; set; }  // Ngày bắt đầu
        public DateTime EndDate { get; set; }  // Ngày kết thúc

        public string Status { get; set; }

        public int RemainingQuantity { get; set; }

        public string VoucherType { get; set; } // "Discount" hoặc "Premium"
    }
}
