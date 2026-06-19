using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;

namespace Nhom9_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class ThuongHieuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThuongHieuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ThuongHieus.ToListAsync());
        }

        public IActionResult Tao() => View(new ThuongHieu { HienThi = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tao(ThuongHieu model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.ThuongHieus.Add(model);
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Thêm thương hiệu thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sua(int id)
        {
            var th = await _context.ThuongHieus.FindAsync(id);
            if (th == null) return NotFound();
            return View(th);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, ThuongHieu model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var th = await _context.ThuongHieus.FindAsync(id);
            if (th == null) return NotFound();

            th.TenThuongHieu = model.TenThuongHieu;
            th.MoTa = model.MoTa;
            th.Logo = model.Logo;
            th.QuocGia = model.QuocGia;
            th.HienThi = model.HienThi;
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Cập nhật thương hiệu thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            var th = await _context.ThuongHieus.FindAsync(id);
            if (th != null)
            {
                _context.ThuongHieus.Remove(th);
                await _context.SaveChangesAsync();
                TempData["ThanhCong"] = "Đã xóa thương hiệu.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
