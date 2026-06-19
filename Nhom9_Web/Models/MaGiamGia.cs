namespace Nhom9_Web.Models
{
    public class MaGiamGia
    {
        public int Id { get; set; }

        public string MaCode { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public int PhanTramGiam { get; set; }

        public decimal? GiamToiDa { get; set; }

        public decimal? DonHangToiThieu { get; set; }

        public int SoLuongSuDung { get; set; }

        public int SoLuongDaDung { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool DangHoatDong { get; set; } = true;

        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
