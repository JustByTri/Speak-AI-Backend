using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interface;
using Common.DTO;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VoucherService> _logger;

        public VoucherService(IUnitOfWork unitOfWork, ILogger<VoucherService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<VoucherResponseDTO> GetVoucherById(Guid voucherId)
        {
            var voucher = await _unitOfWork.Voucher.GetVoucherById(voucherId);

            if (voucher == null) return null;

            return new VoucherResponseDTO
            {
                VoucherId = voucher.VoucherId,
                VoucherCode = voucher.VoucherCode,
                Description = voucher.Description,
                DiscountPercentage = (decimal?)voucher.DiscountPercentage,
                IsActive = voucher.IsActive,
                StartDate = voucher.StartDate,
                EndDate = voucher.EndDate,
                Status = voucher.Status,
                RemainingQuantity = voucher.RemainingQuantity
            };
        }


        public async Task<Voucher> GetVoucherByCode(string voucherCode)
        {
            return await _unitOfWork.Voucher.GetVoucherByCode(voucherCode);
        }

        public async Task<List<VoucherResponseDTO>> GetAllVouchers()
        {
            var vouchers = await _unitOfWork.Voucher.GetAllVouchers();

            return vouchers.Select(v => new VoucherResponseDTO
            {
                VoucherId = v.VoucherId,
                VoucherCode = v.VoucherCode,
                Description = v.Description,
                DiscountPercentage = (decimal?)v.DiscountPercentage,
                IsActive = v.IsActive,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Status = v.Status,
                RemainingQuantity = v.RemainingQuantity
            }).ToList();
        }


        public async Task<Voucher> AddVoucherFromDTO(VoucherDTO voucherDTO)
        {
            await _unitOfWork.Voucher.AddVoucherFromDTO(voucherDTO);
            await _unitOfWork.SaveChangeAsync();
            return await _unitOfWork.Voucher.GetVoucherByCode(voucherDTO.VoucherCode);
        }

        public async Task UpdateVoucherFromDTO(Guid voucherId, UpdateVoucherDTO updateDTO)
        {
            if (updateDTO == null) throw new ArgumentNullException(nameof(updateDTO));

            await _unitOfWork.Voucher.UpdateVoucher(voucherId, updateDTO);
            await _unitOfWork.SaveChangeAsync();
        }





        public async Task RemoveVoucher(Guid voucherId)
        {
            await _unitOfWork.Voucher.RemoveVoucher(voucherId);
            await _unitOfWork.SaveChangeAsync();
        }

        //public async Task<List<Voucher>> CheckAndDisableVouchersAsync()
        //{
        //    _logger.LogInformation("Checking for expired or depleted vouchers...");

        //    var vouchers = await _unitOfWork.Voucher.GetAllVouchers();
        //    var now = DateTime.UtcNow.AddHours(7);
        //    _logger.LogInformation($"Current system time (UTC+7): {now}");

        //    var expiredVouchers = new List<Voucher>();

        //    foreach (var voucher in vouchers)
        //    {
        //        if (voucher.EndDate <= now || voucher.RemainingQuantity <= 0)
        //        {
        //            if (voucher.IsActive)
        //            {
        //                voucher.IsActive = false;
        //                voucher.Status = false;
        //                expiredVouchers.Add(voucher);
        //            }
        //        }
        //    }

        //    if (expiredVouchers.Any())
        //    {
        //        foreach (var voucher in expiredVouchers)
        //        {
        //            await _unitOfWork.Voucher.UpdateVoucherStatusAsync(voucher);
        //        }
        //        await _unitOfWork.SaveChangeAsync();

        //        _logger.LogInformation($"{expiredVouchers.Count} vouchers have been disabled.");
        //    }

        //    return expiredVouchers;
        //}


        public async Task<List<Voucher>> CheckAndDisableVouchersAsync()
        {
            _logger.LogInformation("Checking for expired or depleted vouchers...");

            var now = DateTime.UtcNow.AddHours(7);
            _logger.LogInformation($"Current system time (UTC+7): {now}");

            // Lọc trực tiếp trên DB để tối ưu hiệu suất
            var expiredVouchers = (await _unitOfWork.Voucher.GetAllVouchers())
    .Where(v => (v.EndDate <= now || v.RemainingQuantity <= 0) && v.IsActive)
    .ToList();


            if (!expiredVouchers.Any())
            {
                _logger.LogInformation("No vouchers need to be disabled.");
                return new List<Voucher>();
            }

            // Cập nhật trạng thái của tất cả các voucher hết hạn
            foreach (var voucher in expiredVouchers)
            {
                voucher.IsActive = false;
                voucher.Status = false;
            }

            // Chỉ gọi SaveChangeAsync một lần duy nhất
            await _unitOfWork.SaveChangeAsync();

            _logger.LogInformation($"{expiredVouchers.Count} vouchers have been disabled.");
            return expiredVouchers;
        }













    }

}
