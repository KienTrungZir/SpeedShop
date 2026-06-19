namespace Nhom9_Web.Models
{
    public class SanPham
    {
        public int Id { get; set; }

        public string TenSanPham { get; set; } = string.Empty;

        public string? MoTaNgan { get; set; }

        public string? MoTaChiTiet { get; set; }

        public decimal GiaGoc { get; set; }

        public decimal GiaBan { get; set; }

        public int PhanTramGiamGia { get; set; }

        public int SoLuongTon { get; set; }

        public int DanhMucId { get; set; }

        public DanhMuc DanhMuc { get; set; } = null!;

        public int ThuongHieuId { get; set; }

        public ThuongHieu ThuongHieu { get; set; } = null!;

        public string? AnhDaiDien { get; set; }

        public string? TrongLuong { get; set; }

        public string? DoCungVot { get; set; }

        public string? ChatLieu { get; set; }

        public string? MauSac { get; set; }

        public string? KichThuoc { get; set; }

        public bool NoiBat { get; set; }

        public bool HienThi { get; set; } = true;

        public int LuotXem { get; set; }

        public int LuotBan { get; set; }

        public decimal? DiemDanhGia { get; set; }

        public int SoLuotDanhGia { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        public DateTime? NgayCapNhat { get; set; }

        public ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; } = new List<HinhAnhSanPham>();

        public ICollection<BienTheSanPham> BienTheSanPhams { get; set; } = new List<BienTheSanPham>();

        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        public ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();
    }
}
