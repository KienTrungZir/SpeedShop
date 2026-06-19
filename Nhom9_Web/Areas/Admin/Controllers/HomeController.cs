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
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TongSanPham = await _context.SanPhams.CountAsync();
            ViewBag.TongDonHang = await _context.DonHangs.CountAsync();
            ViewBag.DonChoXuLy = await _context.DonHangs.CountAsync(d => d.TrangThai == TrangThaiDonHang.ChoXuLy);
            ViewBag.TongKhachHang = await _context.Users.CountAsync();
            ViewBag.DoanhThu = await _context.DonHangs
                .Where(d => d.TrangThai != TrangThaiDonHang.DaHuy)
                .SumAsync(d => d.TongThanhToan);

            ViewBag.DonHangMoi = await _context.DonHangs
                .OrderByDescending(d => d.NgayDat)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
