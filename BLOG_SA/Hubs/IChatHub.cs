namespace BLOG_SA.Hubs
{
    public interface IChatHub
    {
        Task ReceiveConnectedClientCountAllClient(int clientCount);
        Task ReceiveMessageForGroupClients(string message,string nickName);
    }
}
