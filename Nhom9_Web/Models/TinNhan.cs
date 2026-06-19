namespace Nhom9_Web.Models
{
    public class TinNhan
    {
        public int Id { get; set; }

        public string NguoiGuiId { get; set; } = string.Empty;

        public ApplicationUser NguoiGui { get; set; } = null!;

        public string NguoiNhanId { get; set; } = string.Empty;

        public ApplicationUser NguoiNhan { get; set; } = null!;

        public string NoiDung { get; set; } = string.Empty;

        public bool DaDoc { get; set; }

        public DateTime NgayGui { get; set; } = DateTime.UtcNow;
    }
}
