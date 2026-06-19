namespace Nhom9_Web.ViewModels
{
    public class GioHangViewModel
    {
        public List<GioHangChiTietViewModel> Items { get; set; } = new();
        public decimal TongTien => Items.Sum(i => i.ThanhTien);
        public int TongSoLuong => Items.Sum(i => i.SoLuong);
    }
}
