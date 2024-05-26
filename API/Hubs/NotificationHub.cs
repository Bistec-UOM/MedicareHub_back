using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

public interface INotificationClient
{
    Task ReceiveNotification(string message);
    Task Receiver(string message);
}

public class NotificationHub : Hub<INotificationClient>
{

    public NotificationHub(ApplicationDbContext dbContext)
    {
    }

    public override async Task OnConnectedAsync()
    {
            await Clients.All.ReceiveNotification($"Thank you for connecting,: {Context.ConnectionId}");
            await base.OnConnectedAsync();
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

}
