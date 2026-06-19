using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nhom9_Web.Data;
using Nhom9_Web.Models;
using Nhom9_Web.ViewModels;

namespace Nhom9_Web.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HoiThoaiViewModel>> LayDanhSachHoiThoaiAsync(string userId, bool laAdmin)
        {
            var tinNhans = await _context.TinNhans
                .Include(t => t.NguoiGui)
                .Include(t => t.NguoiNhan)
                .Where(t => t.NguoiGuiId == userId || t.NguoiNhanId == userId)
                .OrderByDescending(t => t.NgayGui)
                .ToListAsync();

            var doiTacIds = tinNhans
                .Select(t => t.NguoiGuiId == userId ? t.NguoiNhanId : t.NguoiGuiId)
                .Distinct()
                .ToList();

            if (!laAdmin)
            {
                var admin = await LayAdminAsync();
                if (admin != null && !doiTacIds.Contains(admin.Id))
                    doiTacIds.Add(admin.Id);
            }

            var hoiThoais = new List<HoiThoaiViewModel>();

            foreach (var doiTacId in doiTacIds)
            {
                var doiTac = await _context.Users.FindAsync(doiTacId);
                if (doiTac == null) continue;

                var tinTrongHoiThoai = tinNhans
                    .Where(t => t.NguoiGuiId == doiTacId || t.NguoiNhanId == doiTacId)
                    .ToList();

                var tinCuoi = tinTrongHoiThoai.FirstOrDefault();

                hoiThoais.Add(new HoiThoaiViewModel
                {
                    DoiTacId = doiTac.Id,
                    DoiTacTen = doiTac.HoTen,
                    DoiTacEmail = doiTac.Email,
                    TinNhanCuoi = tinCuoi?.NoiDung,
                    ThoiGianCuoi = tinCuoi?.NgayGui,
                    SoTinChuaDoc = tinTrongHoiThoai.Count(t => t.NguoiNhanId == userId && !t.DaDoc)
                });
            }

            return hoiThoais
                .OrderByDescending(h => h.ThoiGianCuoi ?? DateTime.MinValue)
                .ThenBy(h => h.DoiTacTen)
                .ToList();
        }

        public async Task<List<TinNhanViewModel>> LayLichSuTinNhanAsync(string userId, string doiTacId)
        {
            await DanhDauDaDocAsync(userId, doiTacId);

            return await _context.TinNhans
                .Include(t => t.NguoiGui)
                .Where(t =>
                    (t.NguoiGuiId == userId && t.NguoiNhanId == doiTacId) ||
                    (t.NguoiGuiId == doiTacId && t.NguoiNhanId == userId))
                .OrderBy(t => t.NgayGui)
                .Select(t => new TinNhanViewModel
                {
                    Id = t.Id,
                    NguoiGuiId = t.NguoiGuiId,
                    NguoiNhanId = t.NguoiNhanId,
                    NguoiGuiTen = t.NguoiGui.HoTen,
                    NoiDung = t.NoiDung,
                    NgayGui = t.NgayGui,
                    LaCuaToi = t.NguoiGuiId == userId
                })
                .ToListAsync();
        }

        public async Task<TinNhanViewModel?> LuuTinNhanAsync(string nguoiGuiId, string nguoiNhanId, string noiDung)
        {
            if (string.IsNullOrWhiteSpace(noiDung)) return null;

            var nguoiGui = await _context.Users.FindAsync(nguoiGuiId);
            var nguoiNhan = await _context.Users.FindAsync(nguoiNhanId);
            if (nguoiGui == null || nguoiNhan == null) return null;

            var tinNhan = new TinNhan
            {
                NguoiGuiId = nguoiGuiId,
                NguoiNhanId = nguoiNhanId,
                NoiDung = noiDung.Trim(),
                DaDoc = false,
                NgayGui = DateTime.UtcNow
            };

            _context.TinNhans.Add(tinNhan);
            await _context.SaveChangesAsync();

            return new TinNhanViewModel
            {
                Id = tinNhan.Id,
                NguoiGuiId = tinNhan.NguoiGuiId,
                NguoiNhanId = tinNhan.NguoiNhanId,
                NguoiGuiTen = nguoiGui.HoTen,
                NoiDung = tinNhan.NoiDung,
                NgayGui = tinNhan.NgayGui,
                LaCuaToi = true
            };
        }

        public async Task DanhDauDaDocAsync(string userId, string doiTacId)
        {
            var chuaDoc = await _context.TinNhans
                .Where(t => t.NguoiGuiId == doiTacId && t.NguoiNhanId == userId && !t.DaDoc)
                .ToListAsync();

            if (!chuaDoc.Any()) return;

            foreach (var tin in chuaDoc)
                tin.DaDoc = true;

            await _context.SaveChangesAsync();
        }

        public async Task<int> DemTinChuaDocAsync(string userId)
        {
            try
            {
                return await _context.TinNhans
                    .CountAsync(t => t.NguoiNhanId == userId && !t.DaDoc);
            }
            catch (SqlException)
            {
                return 0;
            }
        }

        public async Task<ApplicationUser?> LayAdminAsync()
        {
            var admins = await _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .Where(x => x.Name == AppRoles.Admin)
                .Select(x => x.UserId)
                .ToListAsync();

            if (!admins.Any()) return null;

            return await _context.Users.FirstOrDefaultAsync(u => admins.Contains(u.Id));
        }

        public async Task<ApplicationUser?> LayNguoiDungAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<ApplicationUser?> LayDoiTacAsync(string userId)
        {
            return await LayNguoiDungAsync(userId);
        }

        public async Task<bool> CoQuyenNhanTinAsync(string userId, string doiTacId, bool laAdmin)
        {
            if (userId == doiTacId) return false;

            var doiTac = await _context.Users.FindAsync(doiTacId);
            if (doiTac == null) return false;

            if (laAdmin) return true;

            var admin = await LayAdminAsync();
            if (admin != null && doiTacId == admin.Id) return true;

            return await _context.TinNhans.AnyAsync(t =>
                (t.NguoiGuiId == userId && t.NguoiNhanId == doiTacId) ||
                (t.NguoiGuiId == doiTacId && t.NguoiNhanId == userId));
        }
    }
}
