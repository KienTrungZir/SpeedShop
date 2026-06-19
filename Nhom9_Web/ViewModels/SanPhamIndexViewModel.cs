using Nhom9_Web.Models;

namespace Nhom9_Web.ViewModels
{
    public class SanPhamIndexViewModel
    {
        public List<SanPhamItemViewModel> SanPhams { get; set; } = new();
        public List<DanhMuc> DanhMucs { get; set; } = new();
        public List<ThuongHieu> ThuongHieus { get; set; } = new();
        public int? DanhMucId { get; set; }
        public int? ThuongHieuId { get; set; }
        public string? TuKhoa { get; set; }
        public int Trang { get; set; } = 1;
        public int TongTrang { get; set; } = 1;
    }
}
