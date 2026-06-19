namespace Nhom9_Web.ViewModels
{
    public class HoiThoaiViewModel
    {
        public string DoiTacId { get; set; } = string.Empty;
        public string DoiTacTen { get; set; } = string.Empty;
        public string? DoiTacEmail { get; set; }
        public string? TinNhanCuoi { get; set; }
        public DateTime? ThoiGianCuoi { get; set; }
        public int SoTinChuaDoc { get; set; }
    }
}
