using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nhom9_Web.Helpers;
using Nhom9_Web.Models;
using Nhom9_Web.Services;
using Nhom9_Web.ViewModels;

namespace Nhom9_Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, ChatHelper.TaoNhomNguoiDung(userId));
            }

            await base.OnConnectedAsync();
        }

        public async Task JoinChat(string doiTacId)
        {
            var userId = LayUserId();
            if (userId == null) return;

            var laAdmin = Context.User!.IsInRole(AppRoles.Admin);
            if (!await _chatService.CoQuyenNhanTinAsync(userId, doiTacId, laAdmin))
                throw new HubException("Bạn không có quyền tham gia cuộc trò chuyện này.");

            await Groups.AddToGroupAsync(Context.ConnectionId, ChatHelper.TaoNhomChat(userId, doiTacId));
            await _chatService.DanhDauDaDocAsync(userId, doiTacId);
        }

        public async Task SendMessage(string nguoiNhanId, string noiDung)
        {
            var nguoiGuiId = LayUserId();
            if (nguoiGuiId == null) return;

            var laAdmin = Context.User!.IsInRole(AppRoles.Admin);
            if (!await _chatService.CoQuyenNhanTinAsync(nguoiGuiId, nguoiNhanId, laAdmin))
                throw new HubException("Không thể gửi tin nhắn tới người này.");

            var daLuu = await _chatService.LuuTinNhanAsync(nguoiGuiId, nguoiNhanId, noiDung);
            if (daLuu == null) throw new HubException("Nội dung tin nhắn không hợp lệ.");

            var payload = new TinNhanViewModel
            {
                Id = daLuu.Id,
                NguoiGuiId = daLuu.NguoiGuiId,
                NguoiNhanId = daLuu.NguoiNhanId,
                NguoiGuiTen = daLuu.NguoiGuiTen,
                NoiDung = daLuu.NoiDung,
                NgayGui = daLuu.NgayGui
            };

            var nhomChat = ChatHelper.TaoNhomChat(nguoiGuiId, nguoiNhanId);
            await Clients.Group(nhomChat).SendAsync("ReceiveMessage", payload);
            await Clients.Group(ChatHelper.TaoNhomNguoiDung(nguoiNhanId))
                .SendAsync("TinNhanMoi", payload);
        }

        private string? LayUserId() => Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
