namespace Nhom9_Web.ViewModels
{
    public class TroChuyenViewModel
    {
        public string NguoiDungId { get; set; } = string.Empty;
        public string NguoiDungTen { get; set; } = string.Empty;
        public string DoiTacId { get; set; } = string.Empty;
        public string DoiTacTen { get; set; } = string.Empty;
        public List<TinNhanViewModel> TinNhans { get; set; } = new();
    }
}
