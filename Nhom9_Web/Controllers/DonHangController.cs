using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;
using Nhom9_Web.Models.Enums;
using Nhom9_Web.Services;
using Nhom9_Web.ViewModels;

namespace Nhom9_Web.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GioHangSessionService _gioHang;

        public DonHangController(ApplicationDbContext context, GioHangSessionService gioHang)
        {
            _context = context;
            _gioHang = gioHang;
        }

        [Authorize]
        public async Task<IActionResult> LichSu()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var donHangs = await _context.DonHangs
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            return View(donHangs);
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.SanPham)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donHang == null) return NotFound();

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (donHang.UserId != userId) return Forbid();
            }

            return View(donHang);
        }

        [Authorize]
        public async Task<IActionResult> ThanhToan()
        {
            var gioHang = await _gioHang.LayChiTietAsync();
            if (!gioHang.Items.Any())
            {
                TempData["Loi"] = "Giỏ hàng trống.";
                return RedirectToAction("Index", "GioHang");
            }

            var model = new ThanhToanViewModel { GioHang = gioHang };

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user != null)
                {
                    model.HoTenNguoiNhan = user.HoTen;
                    model.SoDienThoai = user.PhoneNumber ?? string.Empty;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ThanhToan(ThanhToanViewModel model)
        {
            model.GioHang = await _gioHang.LayChiTietAsync();

            if (!model.GioHang.Items.Any())
            {
                TempData["Loi"] = "Giỏ hàng trống.";
                return RedirectToAction("Index", "GioHang");
            }

            if (!ModelState.IsValid) return View(model);

            foreach (var item in model.GioHang.Items)
            {
                if (item.SoLuong > item.SoLuongTon)
                {
                    ModelState.AddModelError("", $"{item.TenSanPham} chỉ còn {item.SoLuongTon} trong kho.");
                    return View(model);
                }
            }

            decimal giamGia = 0;
            MaGiamGia? maGiamGia = null;

            if (!string.IsNullOrWhiteSpace(model.MaGiamGia))
            {
                maGiamGia = await _context.MaGiamGias
                    .FirstOrDefaultAsync(m => m.MaCode == model.MaGiamGia && m.DangHoatDong
                        && m.NgayBatDau <= DateTime.UtcNow && m.NgayKetThuc >= DateTime.UtcNow
                        && m.SoLuongDaDung < m.SoLuongSuDung);

                if (maGiamGia == null)
                {
                    ModelState.AddModelError(nameof(model.MaGiamGia), "Mã giảm giá không hợp lệ hoặc đã hết hạn.");
                    return View(model);
                }

                if (maGiamGia.DonHangToiThieu.HasValue && model.GioHang.TongTien < maGiamGia.DonHangToiThieu)
                {
                    ModelState.AddModelError(nameof(model.MaGiamGia),
                        $"Đơn hàng tối thiểu {maGiamGia.DonHangToiThieu:N0}đ để dùng mã này.");
                    return View(model);
                }

                giamGia = model.GioHang.TongTien * maGiamGia.PhanTramGiam / 100;
                if (maGiamGia.GiamToiDa.HasValue && giamGia > maGiamGia.GiamToiDa)
                    giamGia = maGiamGia.GiamToiDa.Value;

                model.GiamGia = giamGia;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var donHang = new DonHang
            {
                MaDonHang = $"DH{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(100, 999)}",
                UserId = userId,
                HoTenNguoiNhan = model.HoTenNguoiNhan,
                SoDienThoai = model.SoDienThoai,
                DiaChiGiao = model.DiaChiGiao,
                PhuongXa = model.PhuongXa,
                QuanHuyen = model.QuanHuyen,
                TinhThanh = model.TinhThanh,
                TongTienHang = model.GioHang.TongTien,
                PhiVanChuyen = model.PhiVanChuyen,
                GiamGia = giamGia,
                TongThanhToan = model.GioHang.TongTien + model.PhiVanChuyen - giamGia,
                PhuongThucThanhToan = model.PhuongThucThanhToan,
                MaGiamGiaId = maGiamGia?.Id,
                GhiChu = model.GhiChu,
                TrangThai = TrangThaiDonHang.ChoXuLy
            };

            foreach (var item in model.GioHang.Items)
            {
                donHang.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    SanPhamId = item.SanPhamId,
                    BienTheSanPhamId = item.BienTheSanPhamId,
                    TenSanPham = item.TenSanPham,
                    TenBienThe = item.TenBienThe,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia
                });

                var sanPham = await _context.SanPhams
                    .Include(s => s.BienTheSanPhams)
                    .FirstAsync(s => s.Id == item.SanPhamId);

                if (item.BienTheSanPhamId.HasValue)
                {
                    var bienThe = sanPham.BienTheSanPhams.First(b => b.Id == item.BienTheSanPhamId);
                    bienThe.SoLuongTon -= item.SoLuong;
                }
                else
                {
                    sanPham.SoLuongTon -= item.SoLuong;
                }

                sanPham.LuotBan += item.SoLuong;
            }

            if (maGiamGia != null)
                maGiamGia.SoLuongDaDung++;

            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();

            _gioHang.XoaTatCa();
            TempData["ThanhCong"] = $"Đặt hàng thành công! Mã đơn: {donHang.MaDonHang}";
            return RedirectToAction(nameof(ChiTiet), new { id = donHang.Id });
        }
    }
}
