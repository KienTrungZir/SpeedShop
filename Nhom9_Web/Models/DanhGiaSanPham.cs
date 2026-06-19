namespace Nhom9_Web.Models
{
    public class DanhGiaSanPham
    {
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        public SanPham SanPham { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public int Diem { get; set; }

        public string? BinhLuan { get; set; }

        public DateTime NgayDanhGia { get; set; } = DateTime.UtcNow;

        public bool HienThi { get; set; } = true;
    }
}
