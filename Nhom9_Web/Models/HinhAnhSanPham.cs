using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nhom9_Web.Models
{
    public class HinhAnhSanPham
    {
        public int Id { get; set; }

        public int SanPhamId { get; set; }

        [ValidateNever]
        public SanPham SanPham { get; set; } = null!;

        public string DuongDanAnh { get; set; } = string.Empty;

        public int ThuTu { get; set; }

        public bool LaAnhChinh { get; set; }
    }
}

