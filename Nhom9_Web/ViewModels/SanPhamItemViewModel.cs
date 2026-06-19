namespace Nhom9_Web.ViewModels
{
    public class SanPhamItemViewModel
    {
        public int Id { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public decimal GiaBan { get; set; }
        public int PhanTramGiamGia { get; set; }
        public string? AnhDaiDien { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public string TenThuongHieu { get; set; } = string.Empty;
        public decimal? DiemDanhGia { get; set; }
        public bool NoiBat { get; set; }
    }
}
