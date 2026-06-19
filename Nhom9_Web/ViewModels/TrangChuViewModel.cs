using Nhom9_Web.Models;

namespace Nhom9_Web.ViewModels
{
    public class TrangChuViewModel
    {
        public List<SanPhamItemViewModel> SanPhamNoiBat { get; set; } = new();
        public List<SanPhamItemViewModel> SanPhamMoi { get; set; } = new();
        public List<DanhMuc> DanhMucs { get; set; } = new();
    }
}
