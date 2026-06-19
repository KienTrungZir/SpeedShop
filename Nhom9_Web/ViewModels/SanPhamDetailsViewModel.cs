using Nhom9_Web.Models;

namespace Nhom9_Web.ViewModels
{
    public class SanPhamDetailsViewModel
    {
        public SanPham SanPham { get; set; } = null!;
        public List<DanhGiaSanPham> DanhGias { get; set; } = new();
        public List<SanPhamItemViewModel> SanPhamLienQuan { get; set; } = new();
    }
}
