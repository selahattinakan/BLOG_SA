using Microsoft.AspNetCore.SignalR;

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
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).ReceiveMessageForGroupClients($"{nickName} sohbete katıldı", "Admin");
        }

        public async Task BroadcastMessageToGroupClient(string groupName, string message, string nickName)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message, nickName);
        }
    }
}
