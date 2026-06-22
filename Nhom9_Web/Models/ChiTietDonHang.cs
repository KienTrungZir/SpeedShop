using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nhom9_Web.Models
{
    public class ChiTietDonHang
    {
        public int Id { get; set; }

        public int DonHangId { get; set; }

        [ValidateNever]
        public DonHang DonHang { get; set; } = null!;

        public int SanPhamId { get; set; }

        [ValidateNever]
        public SanPham SanPham { get; set; } = null!;

        public int? BienTheSanPhamId { get; set; }

        [ValidateNever]
        public BienTheSanPham? BienTheSanPham { get; set; }

        public string TenSanPham { get; set; } = string.Empty;

        public string? TenBienThe { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public decimal ThanhTien => SoLuong * DonGia;
    }
}

