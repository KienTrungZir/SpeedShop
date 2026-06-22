using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nhom9_Web.Models
{
    public class ChiTietGioHang
    {
        public int Id { get; set; }

        public int GioHangId { get; set; }

        [ValidateNever]
        public GioHang GioHang { get; set; } = null!;

        public int SanPhamId { get; set; }

        [ValidateNever]
        public SanPham SanPham { get; set; } = null!;

        public int? BienTheSanPhamId { get; set; }

        [ValidateNever]
        public BienTheSanPham? BienTheSanPham { get; set; }

        public int SoLuong { get; set; } = 1;

        public decimal DonGia { get; set; }

        public DateTime NgayThem { get; set; } = DateTime.UtcNow;
    }
}

