namespace Nhom9_Web.Models
{
    public class ChiTietGioHang
    {
        public int Id { get; set; }

        public int GioHangId { get; set; }

        public GioHang GioHang { get; set; } = null!;

        public int SanPhamId { get; set; }

        public SanPham SanPham { get; set; } = null!;

        public int? BienTheSanPhamId { get; set; }

        public BienTheSanPham? BienTheSanPham { get; set; }

        public int SoLuong { get; set; } = 1;

        public decimal DonGia { get; set; }

        public DateTime NgayThem { get; set; } = DateTime.UtcNow;
    }
}
