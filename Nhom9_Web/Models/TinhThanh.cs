namespace Nhom9_Web.Models
{
    public class TinhThanh
    {
        public int Id { get; set; }

        public string Ten { get; set; } = string.Empty;

        public string? MaTinh { get; set; }

        public ICollection<QuanHuyen> QuanHuyens { get; set; } = new List<QuanHuyen>();

        public ICollection<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();
    }
}
