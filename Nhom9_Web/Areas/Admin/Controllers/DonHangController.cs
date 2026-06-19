using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;
using Nhom9_Web.Models.Enums;

namespace Nhom9_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(TrangThaiDonHang? trangThai)
        {
            var query = _context.DonHangs
                .Include(d => d.User)
                .AsQueryable();

            if (trangThai.HasValue)
                query = query.Where(d => d.TrangThai == trangThai);

            ViewBag.TrangThai = trangThai;
            return View(await query.OrderByDescending(d => d.NgayDat).ToListAsync());
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var donHang = await _context.DonHangs
                .Include(d => d.User)
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.SanPham)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donHang == null) return NotFound();
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThai(int id, TrangThaiDonHang trangThai)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null) return NotFound();

            donHang.TrangThai = trangThai;
            if (trangThai == TrangThaiDonHang.DaGiao || trangThai == TrangThaiDonHang.HoanThanh)
                donHang.NgayGiao = DateTime.UtcNow;
            //
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Cập nhật trạng thái đơn hàng thành công.";
            return RedirectToAction(nameof(ChiTiet), new { id });
        }
    }
}
