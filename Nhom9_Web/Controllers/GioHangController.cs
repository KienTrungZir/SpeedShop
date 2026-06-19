using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Services;

namespace Nhom9_Web.Controllers
{
    public class GioHangController : Controller
    {
        private readonly GioHangSessionService _gioHang;
        private readonly ApplicationDbContext _context;

        public GioHangController(GioHangSessionService gioHang, ApplicationDbContext context)
        {
            _gioHang = gioHang;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _gioHang.LayChiTietAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(int sanPhamId, int? bienTheId, int soLuong = 1)
        {
            var sanPham = await _context.SanPhams
                .Include(s => s.BienTheSanPhams)
                .FirstOrDefaultAsync(s => s.Id == sanPhamId && s.HienThi);

            if (sanPham == null)
            {
                TempData["Loi"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "SanPham");
            }

            int tonKho;
            if (bienTheId.HasValue)
            {
                var bienThe = sanPham.BienTheSanPhams.FirstOrDefault(b => b.Id == bienTheId && b.HienThi);
                if (bienThe == null)
                {
                    TempData["Loi"] = "Biến thể sản phẩm không hợp lệ.";
                    return RedirectToAction("ChiTiet", "SanPham", new { id = sanPhamId });
                }
                tonKho = bienThe.SoLuongTon;
            }
            else
            {
                tonKho = sanPham.SoLuongTon;
            }

            if (soLuong > tonKho)
            {
                TempData["Loi"] = $"Chỉ còn {tonKho} sản phẩm trong kho.";
                return RedirectToAction("ChiTiet", "SanPham", new { id = sanPhamId });
            }

            _gioHang.Them(sanPhamId, bienTheId, soLuong);
            TempData["ThanhCong"] = "Đã thêm vào giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhat(int sanPhamId, int? bienTheId, int soLuong)
        {
            _gioHang.CapNhat(sanPhamId, bienTheId, soLuong);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Xoa(int sanPhamId, int? bienTheId)
        {
            _gioHang.Xoa(sanPhamId, bienTheId);
            return RedirectToAction(nameof(Index));
        }
    }
}
