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
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donHang == null) return NotFound();

            // Nếu đơn hàng chuyển từ trạng thái khác sang Đã Hủy
            if (donHang.TrangThai != TrangThaiDonHang.DaHuy && trangThai == TrangThaiDonHang.DaHuy)
            {
                // Hoàn lại số lượng tồn kho
                foreach (var item in donHang.ChiTietDonHangs)
                {
                    var sp = await _context.SanPhams
                        .Include(s => s.BienTheSanPhams)
                        .FirstOrDefaultAsync(s => s.Id == item.SanPhamId);

                    if (sp != null)
                    {
                        if (item.BienTheSanPhamId.HasValue)
                        {
                            var bienThe = sp.BienTheSanPhams.FirstOrDefault(b => b.Id == item.BienTheSanPhamId);
                            if (bienThe != null) bienThe.SoLuongTon += item.SoLuong;
                        }
                        else
                        {
                            sp.SoLuongTon += item.SoLuong;
                        }
                        
                        // Trừ lại lượt bán (nếu muốn)
                        sp.LuotBan -= item.SoLuong;
                        if (sp.LuotBan < 0) sp.LuotBan = 0;
                    }
                }
            }

            donHang.TrangThai = trangThai;
            if (trangThai == TrangThaiDonHang.DaGiao || trangThai == TrangThaiDonHang.HoanThanh)
                donHang.NgayGiao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Cập nhật trạng thái đơn hàng thành công.";
            return RedirectToAction(nameof(ChiTiet), new { id });
        }
    }
}
