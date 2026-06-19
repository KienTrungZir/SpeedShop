using Microsoft.AspNetCore.Identity;
using Nhom9_Web.Models.Enums;

namespace Nhom9_Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string HoTen { get; set; } = string.Empty;

        public DateTime? NgaySinh { get; set; }

        public GioiTinh? GioiTinh { get; set; }

        public string? AnhDaiDien { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.UtcNow;

        public bool DangHoatDong { get; set; } = true;

        public GioHang? GioHang { get; set; }

        public ICollection<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();

        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

        public ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();

        public ICollection<TinNhan> TinNhanGui { get; set; } = new List<TinNhan>();

        public ICollection<TinNhan> TinNhanNhan { get; set; } = new List<TinNhan>();
    }
}
