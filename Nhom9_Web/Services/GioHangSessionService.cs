using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;
using Nhom9_Web.ViewModels;

namespace Nhom9_Web.Services
{
    public class GioHangSessionService
    {
        private const string SessionKey = "GioHang";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public GioHangSessionService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public List<GioHangItemSession> LayDanhSach()
        {
            var json = Session.GetString(SessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<GioHangItemSession>()
                : JsonSerializer.Deserialize<List<GioHangItemSession>>(json) ?? new List<GioHangItemSession>();
        }

        private void LuuDanhSach(List<GioHangItemSession> items)
        {
            Session.SetString(SessionKey, JsonSerializer.Serialize(items));
        }

        public void Them(int sanPhamId, int? bienTheId, int soLuong)
        {
            var items = LayDanhSach();
            var existing = items.FirstOrDefault(i =>
                i.SanPhamId == sanPhamId && i.BienTheSanPhamId == bienTheId);

            if (existing != null)
            {
                existing.SoLuong += soLuong;
            }
            else
            {
                items.Add(new GioHangItemSession
                {
                    SanPhamId = sanPhamId,
                    BienTheSanPhamId = bienTheId,
                    SoLuong = soLuong
                });
            }

            LuuDanhSach(items);
        }

        public void CapNhat(int sanPhamId, int? bienTheId, int soLuong)
        {
            var items = LayDanhSach();
            var item = items.FirstOrDefault(i =>
                i.SanPhamId == sanPhamId && i.BienTheSanPhamId == bienTheId);

            if (item == null) return;

            if (soLuong <= 0)
            {
                items.Remove(item);
            }
            else
            {
                item.SoLuong = soLuong;
            }

            LuuDanhSach(items);
        }

        public void Xoa(int sanPhamId, int? bienTheId)
        {
            var items = LayDanhSach();
            items.RemoveAll(i => i.SanPhamId == sanPhamId && i.BienTheSanPhamId == bienTheId);
            LuuDanhSach(items);
        }

        public void XoaTatCa()
        {
            Session.Remove(SessionKey);
        }

        public int DemSoLuong() => LayDanhSach().Sum(i => i.SoLuong);

        public async Task<GioHangViewModel> LayChiTietAsync()
        {
            var items = LayDanhSach();
            var viewModel = new GioHangViewModel();

            if (!items.Any()) return viewModel;

            var sanPhamIds = items.Select(i => i.SanPhamId).Distinct().ToList();
            var sanPhams = await _context.SanPhams
                .Include(s => s.BienTheSanPhams)
                .Where(s => sanPhamIds.Contains(s.Id))
                .ToListAsync();

            foreach (var item in items)
            {
                var sp = sanPhams.FirstOrDefault(s => s.Id == item.SanPhamId);
                if (sp == null) continue;

                BienTheSanPham? bienThe = null;
                if (item.BienTheSanPhamId.HasValue)
                {
                    bienThe = sp.BienTheSanPhams.FirstOrDefault(b => b.Id == item.BienTheSanPhamId);
                }

                var donGia = bienThe?.GiaBan ?? sp.GiaBan;
                var tonKho = bienThe?.SoLuongTon ?? sp.SoLuongTon;

                viewModel.Items.Add(new GioHangChiTietViewModel
                {
                    SanPhamId = sp.Id,
                    BienTheSanPhamId = item.BienTheSanPhamId,
                    TenSanPham = sp.TenSanPham,
                    TenBienThe = bienThe?.TenBienThe,
                    AnhDaiDien = sp.AnhDaiDien,
                    DonGia = donGia,
                    SoLuong = item.SoLuong,
                    SoLuongTon = tonKho
                });
            }

            return viewModel;
        }
    }
}
