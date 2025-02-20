using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DTO;
using DAL.Data;
using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly SpeakAIContext _context;

        public VoucherRepository(SpeakAIContext speakAIContext) : base(speakAIContext)
        {
            _context = speakAIContext;
        }

        // Lấy voucher theo mã
        public async Task<Voucher> GetVoucherByCode(string voucherCode)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.VoucherCode == voucherCode);
        }

        // Lấy voucher theo VoucherId
        public async Task<Voucher> GetVoucherById(string voucherId)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.VoucherId == voucherId);
        }

        // Lấy tất cả voucher
        public async Task<List<Voucher>> GetAllVouchers()
        {
            return await _dbSet.ToListAsync();
        }

        // Thêm voucher từ DTO
        // Thêm voucher từ DTO
        public async Task AddVoucherFromDTO(VoucherDTO voucherDTO)
        {
            if (voucherDTO == null) throw new ArgumentNullException(nameof(voucherDTO));

            var voucher = new Voucher
            {
                VoucherId = Guid.NewGuid().ToString(), // Tạo GUID mới
                VoucherCode = voucherDTO.VoucherCode,
                Description = voucherDTO.Description,
                DiscountPercentage = voucherDTO.DiscountPercentage ?? 0,
                IsActive = voucherDTO.IsActive,
                StartDate = voucherDTO.StartDate,
                EndDate = voucherDTO.EndDate,

                //  Gán RemainingQuantity từ DTO (đây là phần bị thiếu)
                RemainingQuantity = voucherDTO.RemainingQuantity,

                // Giá trị mặc định
                DiscountAmount = 0,
                MinPurchaseAmount = 0,
                VoucherType = "Discount",
                UserId = null,
                Status = "Active" // Gán giá trị mặc định cho Status
            };

            await _dbSet.AddAsync(voucher);
            await _context.SaveChangesAsync(); // Lưu vào DB
        }


        // Cập nhật voucher từ DTO
        public async Task UpdateVoucherFromDTO(string voucherId, VoucherDTO voucherDTO)
        {
            if (voucherDTO == null) throw new ArgumentNullException(nameof(voucherDTO));

            var existingVoucher = await _dbSet.FindAsync(voucherId);
            if (existingVoucher == null) throw new KeyNotFoundException("Voucher không tồn tại");

            // Cập nhật dữ liệu
            existingVoucher.Description = voucherDTO.Description;
            existingVoucher.DiscountPercentage = voucherDTO.DiscountPercentage ?? existingVoucher.DiscountPercentage;
            existingVoucher.IsActive = voucherDTO.IsActive;
            existingVoucher.StartDate = voucherDTO.StartDate;
            existingVoucher.EndDate = voucherDTO.EndDate;
            existingVoucher.Status = existingVoucher.Status ?? "Active"; // Đảm bảo Status có giá trị hợp lệ

            _dbSet.Update(existingVoucher);
            await _context.SaveChangesAsync(); // Lưu vào DB
        }

        // Xóa voucher
        public async Task RemoveVoucher(string voucherId)
        {
            var voucher = await _dbSet.FindAsync(voucherId);
            if (voucher != null)
            {
                _dbSet.Remove(voucher);
                await _context.SaveChangesAsync(); // Lưu vào DB
            }
        }

        public async Task<Voucher?> GetVoucherByCodeAsync(string voucherCode)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.VoucherCode == voucherCode);
        }

        public async Task UpdateVoucherStatusAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }
    }
}
