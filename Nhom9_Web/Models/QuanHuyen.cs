namespace Nhom9_Web.Models
{
    public class QuanHuyen
    {
        public int Id { get; set; }

        public string Ten { get; set; } = string.Empty;

        public int TinhThanhId { get; set; }

        public TinhThanh TinhThanh { get; set; } = null!;

        public ICollection<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();
    }
}
