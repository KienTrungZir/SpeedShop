namespace Nhom9_Web.Models
{
    public class GioHang
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        public DateTime NgayCapNhat { get; set; } = DateTime.UtcNow;

        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
