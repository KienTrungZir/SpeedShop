namespace Nhom9_Web.Models
{
    public class DiaChiGiaoHang
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public string HoTenNguoiNhan { get; set; } = string.Empty;

        public string SoDienThoai { get; set; } = string.Empty;

        public string DiaChiChiTiet { get; set; } = string.Empty;

        public int TinhThanhId { get; set; }

        public TinhThanh TinhThanh { get; set; } = null!;

        public int QuanHuyenId { get; set; }

        public QuanHuyen QuanHuyen { get; set; } = null!;

        public string? PhuongXa { get; set; }

        public bool LaMacDinh { get; set; }
    }
}
