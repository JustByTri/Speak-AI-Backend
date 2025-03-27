using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;
using DAL.Entities;

public interface IVoucherService
{
    Task<VoucherResponseDTO> GetVoucherById(Guid voucherId);
    Task<Voucher> GetVoucherByCode(string voucherCode);
    Task<List<VoucherResponseDTO>> GetAllVouchers();
    Task<Voucher> AddVoucherFromDTO(VoucherDTO voucherDTO);
    Task UpdateVoucherFromDTO(Guid voucherId, UpdateVoucherDTO updateDTO);
    Task RemoveVoucher(Guid voucherId);

    Task<List<Voucher>> CheckAndDisableVouchersAsync();

}
