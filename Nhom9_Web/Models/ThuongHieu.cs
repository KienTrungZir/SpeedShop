namespace Nhom9_Web.Models
{
    public class ThuongHieu
    {
        public int Id { get; set; }

        public string TenThuongHieu { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public string? Logo { get; set; }

        public string? QuocGia { get; set; }

        public bool HienThi { get; set; } = true;

        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
