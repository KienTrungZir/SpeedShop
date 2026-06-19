namespace Nhom9_Web.Helpers
{
    public static class ChatHelper
    {
        public static string TaoNhomChat(string userId1, string userId2)
        {
            return string.CompareOrdinal(userId1, userId2) < 0
                ? $"chat_{userId1}_{userId2}"
                : $"chat_{userId2}_{userId1}";
        }

        public static string TaoNhomNguoiDung(string userId) => $"user_{userId}";
    }
}
