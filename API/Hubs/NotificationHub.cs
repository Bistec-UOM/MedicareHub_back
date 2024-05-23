using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Hubs
{
    public class NotificationHub : Hub<InotificationClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification(
                $"Thank you for connecting{Context.User?.Identity?.Name}");
            await base.OnConnectedAsync();
        }
    }
}public interface InotificationClient
{
    Task ReceiveNotification(string message);
}
