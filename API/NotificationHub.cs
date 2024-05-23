using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API


{
    public class NotificationHub:Hub
    {

        public async Task JoinGroup(string doctorId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, doctorId);
        }

        public async Task LeaveGroup(string doctorId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, doctorId);
        }

        public async Task SendNotificationToDoctor(string doctorId, string message)
        {
            await Clients.Group(doctorId).SendAsync("ReceiveNotification", message);
        }

    }
}
