using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nhom9_Web.Models;
using Nhom9_Web.Services;
using Nhom9_Web.ViewModels;

namespace Nhom9_Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = LayUserId();
            var laAdmin = User.IsInRole(AppRoles.Admin);
            var hoiThoais = await _chatService.LayDanhSachHoiThoaiAsync(userId, laAdmin);
            return View(hoiThoais);
        }

        public async Task<IActionResult> TroChuyen(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

            var userId = LayUserId();
            var laAdmin = User.IsInRole(AppRoles.Admin);

            if (!await _chatService.CoQuyenNhanTinAsync(userId, id, laAdmin))
            {
                TempData["Loi"] = "Bạn không có quyền trò chuyện với người này.";
                return RedirectToAction(nameof(Index));
            }

            var doiTac = await _chatService.LayDoiTacAsync(id);
            if (doiTac == null) return NotFound();

            var tinNhans = await _chatService.LayLichSuTinNhanAsync(userId, id);
            var nguoiDung = await _chatService.LayNguoiDungAsync(userId);

            return View(new TroChuyenViewModel
            {
                NguoiDungId = userId,
                NguoiDungTen = nguoiDung?.HoTen ?? "Bạn",
                DoiTacId = doiTac.Id,
                DoiTacTen = doiTac.HoTen,
                TinNhans = tinNhans
            });
        }

        [HttpGet]
        public async Task<IActionResult> DemTinChuaDoc()
        {
            return Json(new { count = await _chatService.DemTinChuaDocAsync(LayUserId()) });
        }

        private string LayUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
