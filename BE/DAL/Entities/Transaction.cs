using DAL.Entities;
using System.ComponentModel.DataAnnotations;

public class Transaction : BaseEntity
{
    [Key]
    public Guid TransactionId { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; }
    public string TransactionInfo { get; set; } = null!;
    public string TransactionNumber { get; set; }
    public string Status { get; set; }
    public DateTime TransactionDate { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionReference { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Order Order { get; set; }

    // 🆕 Cập nhật lại để có thể lưu cả VoucherId & VoucherCode
    public string? VoucherId { get; set; }
    public string? VoucherCode { get; set; } // 🆕 Thêm dòng này
}
