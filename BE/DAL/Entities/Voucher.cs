using DAL.Entities;

public class Voucher
{
    public string VoucherId { get; set; }
    public string VoucherCode { get; set; }
    public string Description { get; set; }
    public decimal DiscountAmount { get; set; }
    public double DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public decimal MinPurchaseAmount { get; set; }
    public string VoucherType { get; set; }
    public Guid? UserId { get; set; }
    public User User { get; set; }
    public string Status { get; set; } = "Active";

    // 🆕 Thêm số lượng voucher còn lại
    public int RemainingQuantity { get; set; }

    public Voucher()
    {
        Status = "active";
        RemainingQuantity = 0; // Giá trị mặc định
    }

    public Voucher(string voucherCode, string description, int? discountPercentage, bool isActive, int remainingQuantity)
    {
        VoucherId = Guid.NewGuid().ToString();
        VoucherCode = voucherCode;
        Description = description;
        DiscountAmount = 0;
        DiscountPercentage = discountPercentage ?? 0;
        IsActive = isActive;
        StartDate = DateTime.Now;
        EndDate = DateTime.Now.AddMonths(1);
        MinPurchaseAmount = 0;
        VoucherType = "Discount";
        UserId = null;
        RemainingQuantity = remainingQuantity; // 🆕 Thêm số lượng voucher còn lại
    }

    // Kiểm tra voucher còn hợp lệ không
    public bool IsVoucherValid(decimal purchaseAmount, DateTime currentDate)
    {
        bool isValid = IsActive &&
                       currentDate >= StartDate &&
                       currentDate <= EndDate &&
                       purchaseAmount >= MinPurchaseAmount &&
                       RemainingQuantity > 0;

        Console.WriteLine($"[DEBUG] Voucher Valid Check: IsActive={IsActive}, StartDate={StartDate}, EndDate={EndDate}, Now={currentDate}, MinPurchaseAmount={MinPurchaseAmount}, PurchaseAmount={purchaseAmount}, RemainingQuantity={RemainingQuantity}, Result={isValid}");

        return isValid;
    }

    // Tính giảm giá
    public decimal CalculateDiscount(decimal purchaseAmount)
    {
        if (DiscountPercentage > 0)
        {
            return purchaseAmount * (decimal)(DiscountPercentage / 100);
        }
        return DiscountAmount > 0 ? DiscountAmount : 0;
    }
}
