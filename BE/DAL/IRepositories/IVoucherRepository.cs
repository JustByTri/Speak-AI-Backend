using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;
using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IVoucherRepository
    {
        Task<Voucher> GetVoucherById(string voucherId);
        Task<Voucher> GetVoucherByCode(string voucherCode);

        Task<List<Voucher>> GetAllVouchers();
        Task AddVoucherFromDTO(VoucherDTO voucherDTO);
        Task UpdateVoucherFromDTO(string voucherId, VoucherDTO voucherDTO);

        Task RemoveVoucher(string voucherId);

        Task<Voucher?> GetVoucherByCodeAsync(string voucherCode);
        Task UpdateVoucherStatusAsync(Voucher voucher);
    }
}
