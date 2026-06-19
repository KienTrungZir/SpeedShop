(function () {
    const currentUserId = document.getElementById('currentUserId')?.value;
    const partnerId = document.getElementById('partnerId')?.value;
    const messagesEl = document.getElementById('chatMessages');
    const form = document.getElementById('chatForm');
    const input = document.getElementById('messageInput');
    const statusEl = document.getElementById('connectionStatus');
    const sendBtn = document.getElementById('sendBtn');

    if (!currentUserId || !partnerId || !messagesEl) return;

    scrollToBottom();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/hubs/chat')
        .withAutomaticReconnect()
        .build();

    connection.on('ReceiveMessage', function (msg) {
        if (msg.nguoiGuiId !== currentUserId && msg.nguoiGuiId !== partnerId) return;
        if (msg.nguoiNhanId !== currentUserId && msg.nguoiNhanId !== partnerId) return;
        appendMessage(msg);
    });

    connection.onreconnecting(() => setStatus('Đang kết nối lại...', 'warning'));
    connection.onreconnected(async () => {
        setStatus('Đã kết nối', 'success');
        await connection.invoke('JoinChat', partnerId);
    });
    connection.onclose(() => setStatus('Mất kết nối', 'danger'));

    form.addEventListener('submit', async function (e) {
        e.preventDefault();
        const text = input.value.trim();
        if (!text) return;

        sendBtn.disabled = true;
        try {
            await connection.invoke('SendMessage', partnerId, text);
            input.value = '';
        } catch (err) {
            alert(err.message || 'Không gửi được tin nhắn.');
        } finally {
            sendBtn.disabled = false;
            input.focus();
        }
    });

    start();

    async function start() {
        try {
            await connection.start();
            await connection.invoke('JoinChat', partnerId);
            setStatus('Đang trò chuyện', 'success');
            input.focus();
        } catch (err) {
            setStatus('Lỗi kết nối', 'danger');
            console.error(err);
        }
    }

    function appendMessage(msg) {
        if (document.querySelector(`[data-id="${msg.id}"]`)) return;

        const isMine = msg.nguoiGuiId === currentUserId;
        const bubble = document.createElement('div');
        bubble.className = `chat-bubble ${isMine ? 'mine' : 'theirs'}`;
        bubble.dataset.id = msg.id;

        const time = new Date(msg.ngayGui).toLocaleString('vi-VN', {
            hour: '2-digit', minute: '2-digit', day: '2-digit', month: '2-digit'
        });

        let html = '';
        if (!isMine && msg.nguoiGuiTen) {
            html += `<small class="chat-sender">${escapeHtml(msg.nguoiGuiTen)}</small>`;
        }
        html += `<div class="chat-text">${escapeHtml(msg.noiDung)}</div>`;
        html += `<small class="chat-time">${time}</small>`;
        bubble.innerHTML = html;

        messagesEl.appendChild(bubble);
        scrollToBottom();
    }

    function scrollToBottom() {
        messagesEl.scrollTop = messagesEl.scrollHeight;
    }

    function setStatus(text, type) {
        if (!statusEl) return;
        statusEl.textContent = text;
        statusEl.className = `small text-${type}`;
    }

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
})();
