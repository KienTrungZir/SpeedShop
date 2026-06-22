using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;

namespace Nhom9_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .Include(s => s.ThuongHieu)
                .OrderByDescending(s => s.NgayTao)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Tao()
        {
            await NapDropdownAsync();
            return View(new SanPham { HienThi = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tao(SanPham model)
        {
            if (!ModelState.IsValid)
            {
                await NapDropdownAsync();
                return View(model);
            }

            model.NgayTao = DateTime.UtcNow;
            if (model.GiaBan <= 0) model.GiaBan = model.GiaGoc;
            _context.SanPhams.Add(model);
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Thêm sản phẩm thành công.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Sua(int id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();
            await NapDropdownAsync();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua([FromRoute] int id, SanPham model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await NapDropdownAsync();
                return View(model);
            }

            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();

            sp.TenSanPham = model.TenSanPham;
            sp.MoTaNgan = model.MoTaNgan;
            sp.MoTaChiTiet = model.MoTaChiTiet;
            sp.GiaGoc = model.GiaGoc;
            sp.GiaBan = model.GiaBan;
            sp.PhanTramGiamGia = model.PhanTramGiamGia;
            sp.SoLuongTon = model.SoLuongTon;
            sp.DanhMucId = model.DanhMucId;
            sp.ThuongHieuId = model.ThuongHieuId;
            sp.AnhDaiDien = model.AnhDaiDien;
            sp.TrongLuong = model.TrongLuong;
            sp.DoCungVot = model.DoCungVot;
            sp.ChatLieu = model.ChatLieu;
            sp.MauSac = model.MauSac;
            sp.KichThuoc = model.KichThuoc;
            sp.NoiBat = model.NoiBat;
            sp.HienThi = model.HienThi;
            sp.NgayCapNhat = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Cập nhật sản phẩm thành công.";
            return RedirectToAction("Index");
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp != null)
            {
                try
                {
                    _context.SanPhams.Remove(sp);
                    await _context.SaveChangesAsync();
                    TempData["ThanhCong"] = "Đã xóa sản phẩm.";
                }
                catch (DbUpdateException)
                {
                    TempData["Loi"] = "Không thể xóa sản phẩm này vì đã phát sinh Đơn hàng hoặc Phiếu nhập kho. Bạn hãy Sửa sản phẩm và tắt trạng thái 'Hiển thị' thay vì xóa.";
                }
            }
            return RedirectToAction("Index");
        }

        private async Task NapDropdownAsync()
        {
            ViewBag.DanhMucId = new SelectList(await _context.DanhMucs.OrderBy(d => d.ThuTu).ToListAsync(), "Id", "TenDanhMuc");
            ViewBag.ThuongHieuId = new SelectList(await _context.ThuongHieus.ToListAsync(), "Id", "TenThuongHieu");
        }
    }
}
