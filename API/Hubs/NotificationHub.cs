using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}

public class NotificationHub : Hub<INotificationClient>
{
    private static readonly ConcurrentDictionary<int, string> _userConnections = new ConcurrentDictionary<int, string>();
    private static readonly ConcurrentDictionary<string, int> _nameToUserId = new ConcurrentDictionary<string, int>();

    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            _userConnections[userId] = Context.ConnectionId;

            var userName = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                _nameToUserId[userName] = userId;
            }
        }

        await Clients.Client(Context.ConnectionId).ReceiveNotification($"Thank you for connecting: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public static string SendMessage(string name)
    {
        if (_nameToUserId.TryGetValue(name, out int userId) && _userConnections.TryGetValue(userId, out string connectionId))
        {
            return $"Hello from the server: {name} with Connection ID:{connectionId}";
        }
        return $"User {name} not found";
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userIdClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            _userConnections.TryRemove(userId, out _);

            var userName = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                _nameToUserId.TryRemove(userName, out _);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public static string GetConnectionId(int userId)
    {
        _userConnections.TryGetValue(userId, out string connectionId);
        return connectionId;
    }
}
