using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;

namespace Nhom9_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class DanhMucController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhMucController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DanhMucs.OrderBy(d => d.ThuTu).ToListAsync());
        }

        public IActionResult Tao() => View(new DanhMuc { HienThi = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tao(DanhMuc model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.DanhMucs.Add(model);
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Thêm danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sua(int id)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();
            return View(dm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, DanhMuc model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();

            dm.TenDanhMuc = model.TenDanhMuc;
            dm.MoTa = model.MoTa;
            dm.AnhDaiDien = model.AnhDaiDien;
            dm.HienThi = model.HienThi;
            dm.ThuTu = model.ThuTu;
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Cập nhật danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm != null)
            {
                _context.DanhMucs.Remove(dm);
                await _context.SaveChangesAsync();
                TempData["ThanhCong"] = "Đã xóa danh mục.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
