using System.Threading.Tasks;
using Common.DTO;
using DAL.Entities;

public interface IVoucherService
{

    Task<Voucher> GetVoucherById(Guid voucherId);
    Task<Voucher> GetVoucherByCode(string voucherCode); 
    Task<List<Voucher>> GetAllVouchers();  
    Task<Voucher> AddVoucherFromDTO(VoucherDTO voucherDTO); 
    Task UpdateVoucherFromDTO(Guid voucherId, VoucherDTO voucherDTO);  
    Task RemoveVoucher(Guid voucherId);  

    Task<bool> DisableExpiredOrDepletedVouchersAsync(); 

    Task CheckAndDisableVouchersAsync();


}

