using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Models;

namespace Nhom9_Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TinhThanh> TinhThanhs => Set<TinhThanh>();
        public DbSet<QuanHuyen> QuanHuyens => Set<QuanHuyen>();
        public DbSet<DanhMuc> DanhMucs => Set<DanhMuc>();
        public DbSet<ThuongHieu> ThuongHieus => Set<ThuongHieu>();
        public DbSet<SanPham> SanPhams => Set<SanPham>();
        public DbSet<HinhAnhSanPham> HinhAnhSanPhams => Set<HinhAnhSanPham>();
        public DbSet<BienTheSanPham> BienTheSanPhams => Set<BienTheSanPham>();
        public DbSet<GioHang> GioHangs => Set<GioHang>();
        public DbSet<ChiTietGioHang> ChiTietGioHangs => Set<ChiTietGioHang>();
        public DbSet<DiaChiGiaoHang> DiaChiGiaoHangs => Set<DiaChiGiaoHang>();
        public DbSet<MaGiamGia> MaGiamGias => Set<MaGiamGia>();
        public DbSet<DonHang> DonHangs => Set<DonHang>();
        public DbSet<ChiTietDonHang> ChiTietDonHangs => Set<ChiTietDonHang>();
        public DbSet<DanhGiaSanPham> DanhGiaSanPhams => Set<DanhGiaSanPham>();
        public DbSet<TinNhan> TinNhans => Set<TinNhan>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.HoTen).HasMaxLength(100).IsRequired();
                entity.Property(u => u.AnhDaiDien).HasMaxLength(500);
            });

            builder.Entity<TinhThanh>(entity =>
            {
                entity.Property(t => t.Ten).HasMaxLength(100).IsRequired();
                entity.Property(t => t.MaTinh).HasMaxLength(10);
            });

            builder.Entity<QuanHuyen>(entity =>
            {
                entity.Property(q => q.Ten).HasMaxLength(100).IsRequired();
                entity.HasOne(q => q.TinhThanh)
                    .WithMany(t => t.QuanHuyens)
                    .HasForeignKey(q => q.TinhThanhId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<DanhMuc>(entity =>
            {
                entity.Property(d => d.TenDanhMuc).HasMaxLength(100).IsRequired();
                entity.Property(d => d.MoTa).HasMaxLength(500);
                entity.Property(d => d.AnhDaiDien).HasMaxLength(500);
            });

            builder.Entity<ThuongHieu>(entity =>
            {
                entity.Property(t => t.TenThuongHieu).HasMaxLength(100).IsRequired();
                entity.Property(t => t.MoTa).HasMaxLength(500);
                entity.Property(t => t.Logo).HasMaxLength(500);
                entity.Property(t => t.QuocGia).HasMaxLength(50);
            });

            builder.Entity<SanPham>(entity =>
            {
                entity.Property(s => s.TenSanPham).HasMaxLength(200).IsRequired();
                entity.Property(s => s.MoTaNgan).HasMaxLength(500);
                entity.Property(s => s.AnhDaiDien).HasMaxLength(500);
                entity.Property(s => s.TrongLuong).HasMaxLength(50);
                entity.Property(s => s.DoCungVot).HasMaxLength(50);
                entity.Property(s => s.ChatLieu).HasMaxLength(100);
                entity.Property(s => s.MauSac).HasMaxLength(50);
                entity.Property(s => s.KichThuoc).HasMaxLength(50);
                entity.Property(s => s.GiaGoc).HasPrecision(18, 0);
                entity.Property(s => s.GiaBan).HasPrecision(18, 0);
                entity.Property(s => s.DiemDanhGia).HasPrecision(3, 2);

                entity.HasOne(s => s.DanhMuc)
                    .WithMany(d => d.SanPhams)
                    .HasForeignKey(s => s.DanhMucId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.ThuongHieu)
                    .WithMany(t => t.SanPhams)
                    .HasForeignKey(s => s.ThuongHieuId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<HinhAnhSanPham>(entity =>
            {
                entity.Property(h => h.DuongDanAnh).HasMaxLength(500).IsRequired();

                entity.HasOne(h => h.SanPham)
                    .WithMany(s => s.HinhAnhSanPhams)
                    .HasForeignKey(h => h.SanPhamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BienTheSanPham>(entity =>
            {
                entity.Property(b => b.TenBienThe).HasMaxLength(100).IsRequired();
                entity.Property(b => b.MauSac).HasMaxLength(50);
                entity.Property(b => b.KichThuoc).HasMaxLength(50);
                entity.Property(b => b.TrongLuong).HasMaxLength(50);
                entity.Property(b => b.DoCungVot).HasMaxLength(50);
                entity.Property(b => b.MaSku).HasMaxLength(50);
                entity.Property(b => b.GiaBan).HasPrecision(18, 0);

                entity.HasOne(b => b.SanPham)
                    .WithMany(s => s.BienTheSanPhams)
                    .HasForeignKey(b => b.SanPhamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<GioHang>(entity =>
            {
                entity.HasOne(g => g.User)
                    .WithOne(u => u.GioHang)
                    .HasForeignKey<GioHang>(g => g.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ChiTietGioHang>(entity =>
            {
                entity.Property(c => c.DonGia).HasPrecision(18, 0);

                entity.HasOne(c => c.GioHang)
                    .WithMany(g => g.ChiTietGioHangs)
                    .HasForeignKey(c => c.GioHangId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.SanPham)
                    .WithMany(s => s.ChiTietGioHangs)
                    .HasForeignKey(c => c.SanPhamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.BienTheSanPham)
                    .WithMany(b => b.ChiTietGioHangs)
                    .HasForeignKey(c => c.BienTheSanPhamId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<DiaChiGiaoHang>(entity =>
            {
                entity.Property(d => d.HoTenNguoiNhan).HasMaxLength(100).IsRequired();
                entity.Property(d => d.SoDienThoai).HasMaxLength(20).IsRequired();
                entity.Property(d => d.DiaChiChiTiet).HasMaxLength(500).IsRequired();
                entity.Property(d => d.PhuongXa).HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.DiaChiGiaoHangs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.TinhThanh)
                    .WithMany(t => t.DiaChiGiaoHangs)
                    .HasForeignKey(d => d.TinhThanhId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.QuanHuyen)
                    .WithMany(q => q.DiaChiGiaoHangs)
                    .HasForeignKey(d => d.QuanHuyenId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MaGiamGia>(entity =>
            {
                entity.Property(m => m.MaCode).HasMaxLength(50).IsRequired();
                entity.Property(m => m.MoTa).HasMaxLength(200);
                entity.Property(m => m.GiamToiDa).HasPrecision(18, 0);
                entity.Property(m => m.DonHangToiThieu).HasPrecision(18, 0);
                entity.HasIndex(m => m.MaCode).IsUnique();
            });

            builder.Entity<DonHang>(entity =>
            {
                entity.Property(d => d.MaDonHang).HasMaxLength(20).IsRequired();
                entity.Property(d => d.HoTenNguoiNhan).HasMaxLength(100).IsRequired();
                entity.Property(d => d.SoDienThoai).HasMaxLength(20).IsRequired();
                entity.Property(d => d.DiaChiGiao).HasMaxLength(500).IsRequired();
                entity.Property(d => d.PhuongXa).HasMaxLength(100);
                entity.Property(d => d.QuanHuyen).HasMaxLength(100);
                entity.Property(d => d.TinhThanh).HasMaxLength(100);
                entity.Property(d => d.GhiChu).HasMaxLength(500);
                entity.Property(d => d.TongTienHang).HasPrecision(18, 0);
                entity.Property(d => d.PhiVanChuyen).HasPrecision(18, 0);
                entity.Property(d => d.GiamGia).HasPrecision(18, 0);
                entity.Property(d => d.TongThanhToan).HasPrecision(18, 0);

                entity.HasIndex(d => d.MaDonHang).IsUnique();

                entity.HasOne(d => d.User)
                    .WithMany(u => u.DonHangs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.MaGiamGia)
                    .WithMany(m => m.DonHangs)
                    .HasForeignKey(d => d.MaGiamGiaId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<ChiTietDonHang>(entity =>
            {
                entity.Property(c => c.TenSanPham).HasMaxLength(200).IsRequired();
                entity.Property(c => c.TenBienThe).HasMaxLength(100);
                entity.Property(c => c.DonGia).HasPrecision(18, 0);
                entity.Ignore(c => c.ThanhTien);

                entity.HasOne(c => c.DonHang)
                    .WithMany(d => d.ChiTietDonHangs)
                    .HasForeignKey(c => c.DonHangId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.SanPham)
                    .WithMany(s => s.ChiTietDonHangs)
                    .HasForeignKey(c => c.SanPhamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.BienTheSanPham)
                    .WithMany(b => b.ChiTietDonHangs)
                    .HasForeignKey(c => c.BienTheSanPhamId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<DanhGiaSanPham>(entity =>
            {
                entity.Property(d => d.BinhLuan).HasMaxLength(1000);

                entity.HasOne(d => d.SanPham)
                    .WithMany(s => s.DanhGiaSanPhams)
                    .HasForeignKey(d => d.SanPhamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.DanhGiaSanPhams)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(d => new { d.SanPhamId, d.UserId }).IsUnique();
            });

            builder.Entity<TinNhan>(entity =>
            {
                entity.Property(t => t.NoiDung).HasMaxLength(2000).IsRequired();

                entity.HasOne(t => t.NguoiGui)
                    .WithMany(u => u.TinNhanGui)
                    .HasForeignKey(t => t.NguoiGuiId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.NguoiNhan)
                    .WithMany(u => u.TinNhanNhan)
                    .HasForeignKey(t => t.NguoiNhanId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(t => new { t.NguoiGuiId, t.NguoiNhanId, t.NgayGui });
            });
        }
    }
}
