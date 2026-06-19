using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.ViewModels;

namespace Nhom9_Web.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 12;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? danhMucId, int? thuongHieuId, string? tuKhoa, int trang = 1)
        {
            var query = _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ThuongHieu)
                .Where(s => s.HienThi);

            if (danhMucId.HasValue)
                query = query.Where(s => s.DanhMucId == danhMucId);

            if (thuongHieuId.HasValue)
                query = query.Where(s => s.ThuongHieuId == thuongHieuId);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
                query = query.Where(s => s.TenSanPham.Contains(tuKhoa));

            var total = await query.CountAsync();
            var sanPhams = await query
                .OrderByDescending(s => s.NgayTao)
                .Skip((trang - 1) * PageSize)
                .Take(PageSize)
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

            var viewModel = new SanPhamIndexViewModel
            {
                SanPhams = sanPhams,
                DanhMucs = await _context.DanhMucs.Where(d => d.HienThi).OrderBy(d => d.ThuTu).ToListAsync(),
                ThuongHieus = await _context.ThuongHieus.Where(t => t.HienThi).ToListAsync(),
                DanhMucId = danhMucId,
                ThuongHieuId = thuongHieuId,
                TuKhoa = tuKhoa,
                Trang = trang,
                TongTrang = (int)Math.Ceiling(total / (double)PageSize)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ThuongHieu)
                .Include(s => s.HinhAnhSanPhams)
                .Include(s => s.BienTheSanPhams.Where(b => b.HienThi))
                .FirstOrDefaultAsync(s => s.Id == id && s.HienThi);

            if (sanPham == null) return NotFound();

            sanPham.LuotXem++;
            await _context.SaveChangesAsync();

            var danhGias = await _context.DanhGiaSanPhams
                .Include(d => d.User)
                .Where(d => d.SanPhamId == id && d.HienThi)
                .OrderByDescending(d => d.NgayDanhGia)
                .Take(10)
                .ToListAsync();

            var lienQuan = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ThuongHieu)
                .Where(s => s.HienThi && s.DanhMucId == sanPham.DanhMucId && s.Id != id)
                .OrderByDescending(s => s.LuotBan)
                .Take(4)
                .Select(s => new SanPhamItemViewModel
                {
                    Id = s.Id,
                    TenSanPham = s.TenSanPham,
                    GiaBan = s.GiaBan,
                    PhanTramGiamGia = s.PhanTramGiamGia,
                    AnhDaiDien = s.AnhDaiDien,
                    TenDanhMuc = s.DanhMuc.TenDanhMuc,
                    TenThuongHieu = s.ThuongHieu.TenThuongHieu,
                    DiemDanhGia = s.DiemDanhGia
                })
                .ToListAsync();

            return View(new SanPhamDetailsViewModel
            {
                SanPham = sanPham,
                DanhGias = danhGias,
                SanPhamLienQuan = lienQuan
            });
        }
    }
}
