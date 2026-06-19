using Nhom9_Web.Models.Enums;

namespace Nhom9_Web.Models
{
    public class DonHang
    {
        public int Id { get; set; }

        public string MaDonHang { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public string HoTenNguoiNhan { get; set; } = string.Empty;

        public string SoDienThoai { get; set; } = string.Empty;

        public string DiaChiGiao { get; set; } = string.Empty;

        public string? PhuongXa { get; set; }

        public string? QuanHuyen { get; set; }

        public string? TinhThanh { get; set; }

        public decimal TongTienHang { get; set; }

        public decimal PhiVanChuyen { get; set; }

        public decimal GiamGia { get; set; }

        public decimal TongThanhToan { get; set; }

        public PhuongThucThanhToan PhuongThucThanhToan { get; set; }

        public TrangThaiDonHang TrangThai { get; set; } = TrangThaiDonHang.ChoXuLy;

        public int? MaGiamGiaId { get; set; }

        public MaGiamGia? MaGiamGia { get; set; }

        public string? GhiChu { get; set; }

        public DateTime NgayDat { get; set; } = DateTime.UtcNow;

        public DateTime? NgayGiao { get; set; }

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
