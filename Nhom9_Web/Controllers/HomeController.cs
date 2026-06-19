using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;
using Nhom9_Web.ViewModels;
using System.Diagnostics;

namespace Nhom9_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new TrangChuViewModel
            {
                DanhMucs = await _context.DanhMucs.Where(d => d.HienThi).OrderBy(d => d.ThuTu).Take(6).ToListAsync(),
                SanPhamNoiBat = await LaySanPhamAsync(noibat: true, 8),
                SanPhamMoi = await LaySanPhamAsync(noibat: false, 8)
            };

            return View(viewModel);
        }

        private async Task<List<SanPhamItemViewModel>> LaySanPhamAsync(bool noibat, int take)
        {
            var query = _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ThuongHieu)
                .Where(s => s.HienThi);

            if (noibat)
                query = query.Where(s => s.NoiBat).OrderByDescending(s => s.LuotBan);
            else
                query = query.OrderByDescending(s => s.NgayTao);

            return await query
                .Take(take)
                .Select(s => new SanPhamItemViewModel
                {
                    Id = s.Id,
                    TenSanPham = s.TenSanPham,
                    GiaBan = s.GiaBan,
                    PhanTramGiamGia = s.PhanTramGiamGia,
                    AnhDaiDien = s.AnhDaiDien,
                    TenDanhMuc = s.DanhMuc.TenDanhMuc,
                    TenThuongHieu = s.ThuongHieu.TenThuongHieu,
                    DiemDanhGia = s.DiemDanhGia,
                    NoiBat = s.NoiBat
                })
                .ToListAsync();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
