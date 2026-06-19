namespace Nhom9_Web.Models
{
    public class DanhMuc
    {
        public int Id { get; set; }

        public string TenDanhMuc { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public string? AnhDaiDien { get; set; }

        public bool HienThi { get; set; } = true;

        public int ThuTu { get; set; }

        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
