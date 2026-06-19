namespace Nhom9_Web.ViewModels
{
    public class GioHangChiTietViewModel
    {
        public int SanPhamId { get; set; }
        public int? BienTheSanPhamId { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public string? TenBienThe { get; set; }
        public string? AnhDaiDien { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongTon { get; set; }
        public decimal ThanhTien => DonGia * SoLuong;
    }
}
