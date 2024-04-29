using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace BLOG_SA.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        private static int ConnectedClientCount = 0;
        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            base.OnDisconnectedAsync(exception);
        }

        public async Task AddGroup(string groupName, string nickName)
        {
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            nickName = rRemScript.Replace(nickName, "");
            nickName = Regex.Replace(
                nickName,
                @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                string.Empty,
                RegexOptions.Singleline | RegexOptions.IgnoreCase
            );

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).ReceiveMessageForGroupClients($"{nickName} sohbete katıldı", "Admin");
        }

        public async Task BroadcastMessageToGroupClient(string groupName, string message, string nickName)
        {
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            nickName = rRemScript.Replace(nickName, "");
            nickName = Regex.Replace(
                nickName,
                @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                string.Empty,
                RegexOptions.Singleline | RegexOptions.IgnoreCase
            );

            message = rRemScript.Replace(message, "");
            message = Regex.Replace(
                message,
                @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                string.Empty,
                RegexOptions.Singleline | RegexOptions.IgnoreCase
            );

            await Clients.Group(groupName).ReceiveMessageForGroupClients(message, nickName);
        }
    }
}
