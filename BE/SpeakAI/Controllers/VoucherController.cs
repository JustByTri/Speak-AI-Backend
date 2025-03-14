using BLL.Interface;
using BLL.Service;
using Common.DTO;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly ILogger<VoucherController> _logger;

        public VoucherController(IVoucherService voucherService, ILogger<VoucherController> logger)
        {
            _voucherService = voucherService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Lấy voucher theo mã
        [HttpGet("{voucherCode}")]
        public async Task<ActionResult<Voucher>> GetVoucherByCode(string voucherCode)
        {
            var voucher = await _voucherService.GetVoucherByCode(voucherCode);
            if (voucher == null)
            {
                return NotFound();
            }
            return Ok(voucher);
        }

        // Lấy tất cả vouchers
        [HttpGet]
        public async Task<ActionResult<List<Voucher>>> GetAllVouchers()
        {
            var vouchers = await _voucherService.GetAllVouchers();
            return Ok(vouchers);
        }

        // ✅ API để kiểm tra và vô hiệu hóa voucher thủ công
        [HttpGet("check-and-disable")]
        public async Task<IActionResult> CheckAndDisableVouchers()
        {
            _logger.LogInformation("API request received to check and disable vouchers.");

            await _voucherService.CheckAndDisableVouchersAsync();

            return Ok(new { message = "Voucher check completed." });
        }


        // Thêm voucher từ DTO
        [HttpPost]
        public async Task<IActionResult> AddVoucher([FromBody] VoucherDTO voucherDTO)
        {
            if (voucherDTO == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var newVoucher = await _voucherService.AddVoucherFromDTO(voucherDTO);
            return CreatedAtAction(nameof(GetVoucherByCode), new { voucherCode = newVoucher.VoucherCode }, newVoucher);
        }

        // Lấy voucher theo VoucherId
        [HttpGet("id/{voucherId}")]
        public async Task<IActionResult> GetVoucherById(Guid voucherId)
        {
            var voucher = await _voucherService.GetVoucherById(voucherId);
            if (voucher == null)
                return NotFound("Voucher không tồn tại.");
            return Ok(voucher);
        }




        // Cập nhật voucher từ DTO
        [HttpPut("{voucherId}")]
        public async Task<IActionResult> UpdateVoucher(Guid voucherId, [FromBody] VoucherDTO voucherDTO)
        {
            if (voucherDTO == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            await _voucherService.UpdateVoucherFromDTO(voucherId, voucherDTO);
            return NoContent(); // HTTP 204
        }

        // Xóa voucher
        [HttpDelete("{voucherId}")]
        public async Task<ActionResult> RemoveVoucher(Guid voucherId)
        {
            await _voucherService.RemoveVoucher(voucherId);
            return NoContent();
        }

    

    }
}
