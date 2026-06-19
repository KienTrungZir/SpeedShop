using System.ComponentModel.DataAnnotations;
using Nhom9_Web.Models.Enums;

namespace Nhom9_Web.ViewModels
{
    public class ThanhToanViewModel
    {
        public GioHangViewModel GioHang { get; set; } = new();

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên người nhận")]
        public string HoTenNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiao { get; set; } = string.Empty;

        [Display(Name = "Phường / Xã")]
        public string? PhuongXa { get; set; }

        [Display(Name = "Quận / Huyện")]
        public string? QuanHuyen { get; set; }

        [Display(Name = "Tỉnh / Thành phố")]
        public string? TinhThanh { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; } = PhuongThucThanhToan.ThanhToanKhiNhanHang;

        [Display(Name = "Mã giảm giá")]
        public string? MaGiamGia { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        public decimal PhiVanChuyen { get; set; } = 30000;

        public decimal GiamGia { get; set; }

        public decimal TongThanhToan => GioHang.TongTien + PhiVanChuyen - GiamGia;
    }
}
