namespace Nhom9_Web.Models
{
    public class BienTheSanPham
    {
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        public SanPham SanPham { get; set; } = null!;

        public string TenBienThe { get; set; } = string.Empty;

        public string? MauSac { get; set; }

        public string? KichThuoc { get; set; }

        public string? TrongLuong { get; set; }

        public string? DoCungVot { get; set; }

        public decimal GiaBan { get; set; }

        public int SoLuongTon { get; set; }

        public string? MaSku { get; set; }

        public bool HienThi { get; set; } = true;

        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}
