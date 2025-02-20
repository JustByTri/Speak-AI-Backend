using System.Threading.Tasks;
using Common.DTO;
using DAL.Entities;

public interface IVoucherService
{

    Task<Voucher> GetVoucherById(string voucherId);
    Task<Voucher> GetVoucherByCode(string voucherCode); // Lấy voucher theo mã
    Task<List<Voucher>> GetAllVouchers();  // Lấy tất cả vouchers
    Task<Voucher> AddVoucherFromDTO(VoucherDTO voucherDTO);  // Thêm voucher (trả về voucher mới)
    Task UpdateVoucherFromDTO(string voucherId, VoucherDTO voucherDTO);  // Cập nhật voucher
    Task RemoveVoucher(string voucherId);  // Xóa voucher


    Task<bool> DisableExpiredOrDepletedVouchersAsync(); // vô hiệu hoá Voucher

    Task CheckAndDisableVouchersAsync();


}

