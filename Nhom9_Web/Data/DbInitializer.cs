using Microsoft.AspNetCore.Identity;
using Nhom9_Web.Models;

namespace Nhom9_Web.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in new[] { AppRoles.Admin, AppRoles.Customer })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            const string adminEmail = "admin@caulongshop.vn";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    HoTen = "Quản trị viên",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
            else if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
            {
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }

            var context = services.GetRequiredService<ApplicationDbContext>();

            if (!context.DanhMucs.Any())
            {
                context.DanhMucs.AddRange(
                    new DanhMuc { TenDanhMuc = "Vợt Cầu Lông", ThuTu = 1, HienThi = true },
                    new DanhMuc { TenDanhMuc = "Giày Cầu Lông", ThuTu = 2, HienThi = true },
                    new DanhMuc { TenDanhMuc = "Túi & Balo Cầu Lông", ThuTu = 3, HienThi = true },
                    new DanhMuc { TenDanhMuc = "Phụ Kiện Cầu Lông", ThuTu = 4, HienThi = true }
                );
                await context.SaveChangesAsync();
            }

            if (!context.ThuongHieus.Any())
            {
                context.ThuongHieus.AddRange(
                    new ThuongHieu { TenThuongHieu = "Yonex", QuocGia = "Nhật Bản", HienThi = true },
                    new ThuongHieu { TenThuongHieu = "Li-Ning", QuocGia = "Trung Quốc", HienThi = true },
                    new ThuongHieu { TenThuongHieu = "Victor", QuocGia = "Đài Loan", HienThi = true }
                );
                await context.SaveChangesAsync();
            }

            if (!context.SanPhams.Any())
            {
                var dmVot = context.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == "Vợt Cầu Lông")?.Id ?? 1;
                var dmGiay = context.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == "Giày Cầu Lông")?.Id ?? 2;
                var dmTui = context.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == "Túi & Balo Cầu Lông")?.Id ?? 3;

                var thYonex = context.ThuongHieus.FirstOrDefault(t => t.TenThuongHieu == "Yonex")?.Id ?? 1;
                var thLining = context.ThuongHieus.FirstOrDefault(t => t.TenThuongHieu == "Li-Ning")?.Id ?? 2;
                var thVictor = context.ThuongHieus.FirstOrDefault(t => t.TenThuongHieu == "Victor")?.Id ?? 3;

                context.SanPhams.AddRange(
                    new SanPham
                    {
                        TenSanPham = "Yonex Astrox 88D Pro",
                        DanhMucId = dmVot,
                        ThuongHieuId = thYonex,
                        GiaGoc = 4500000,
                        GiaBan = 4190000,
                        PhanTramGiamGia = 7,
                        SoLuongTon = 15,
                        AnhDaiDien = "/images/yonex_astrox_88d.jpg",
                        TrongLuong = "3U/4U",
                        DoCungVot = "Cứng",
                        ChatLieu = "HM Graphite",
                        MauSac = "Cam/Đen",
                        NoiBat = true,
                        HienThi = true
                    },
                    new SanPham
                    {
                        TenSanPham = "Li-Ning Tectonic 9",
                        DanhMucId = dmVot,
                        ThuongHieuId = thLining,
                        GiaGoc = 4200000,
                        GiaBan = 3800000,
                        PhanTramGiamGia = 9,
                        SoLuongTon = 10,
                        AnhDaiDien = "/images/lining_tectonic_9.jpg",
                        TrongLuong = "3U/4U",
                        DoCungVot = "Hơi cứng",
                        ChatLieu = "Carbon Fiber",
                        MauSac = "Đen/Trắng",
                        NoiBat = true,
                        HienThi = true
                    },
                    new SanPham
                    {
                        TenSanPham = "Victor Thruster K Ryuga",
                        DanhMucId = dmVot,
                        ThuongHieuId = thVictor,
                        GiaGoc = 4000000,
                        GiaBan = 3600000,
                        PhanTramGiamGia = 10,
                        SoLuongTon = 12,
                        AnhDaiDien = "/images/victor_ryuga.jpg",
                        TrongLuong = "3U/4U",
                        DoCungVot = "Rất cứng",
                        ChatLieu = "High Resilient Modulus Graphite",
                        MauSac = "Đen/Đỏ",
                        NoiBat = true,
                        HienThi = true
                    },
                    new SanPham
                    {
                        TenSanPham = "Yonex Power Cushion 65Z3 C1",
                        DanhMucId = dmGiay,
                        ThuongHieuId = thYonex,
                        GiaGoc = 3100000,
                        GiaBan = 2850000,
                        PhanTramGiamGia = 8,
                        SoLuongTon = 20,
                        AnhDaiDien = "/images/yonex_65z3.jpg",
                        TrongLuong = "300g",
                        ChatLieu = "Da PU, Sợi tổng hợp",
                        MauSac = "Trắng/Xanh",
                        NoiBat = true,
                        HienThi = true
                    },
                    new SanPham
                    {
                        TenSanPham = "Balo Cầu Lông Yonex Pro",
                        DanhMucId = dmTui,
                        ThuongHieuId = thYonex,
                        GiaGoc = 1800000,
                        GiaBan = 1600000,
                        PhanTramGiamGia = 11,
                        SoLuongTon = 8,
                        AnhDaiDien = "/images/yonex_backpack.jpg",
                        TrongLuong = "800g",
                        ChatLieu = "Polyester, PU",
                        MauSac = "Xanh/Đen",
                        NoiBat = false,
                        HienThi = true
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
