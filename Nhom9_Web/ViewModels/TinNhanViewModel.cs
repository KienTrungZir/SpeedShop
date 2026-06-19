namespace Nhom9_Web.ViewModels
{
    public class TinNhanViewModel
    {
        public int Id { get; set; }
        public string NguoiGuiId { get; set; } = string.Empty;
        public string NguoiNhanId { get; set; } = string.Empty;
        public string NguoiGuiTen { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public DateTime NgayGui { get; set; }
        public bool LaCuaToi { get; set; }
    }
}
