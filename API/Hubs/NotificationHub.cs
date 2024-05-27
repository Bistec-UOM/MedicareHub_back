using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public interface INotificationClient
{
    Task ReceiveNotification(string message);
    Task Receiver(string message);
    Task broadcastMessage(string name, string message);
}

public class NotificationHub : Hub<INotificationClient>
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationHub(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveNotification($"Thank you for connecting: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // var connectionId = Context.ConnectionId;
        var connectionId = Context.ConnectionId;
        // Find the user associated with the connectionId
        var user = await _dbContext.users.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
        if (user != null)
        {
            // Update the user's email to dhammika@gmail.com
            user.Email = "dhammika@gmail.com";
            user.ConnectionId = null; // Clear the connection ID

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Broadcast the new email
            await Clients.All.broadcastMessage(user.Name, "User has disconnected.");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public async Task Send(string id, string message)
    {
        if (int.TryParse(id, out int userId))
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.Email = "yasiru@gmail.com"; // Update the user's email to yasiru@gmail.com
                user.ConnectionId = Context.ConnectionId; // Update the user's connection ID
                await _dbContext.SaveChangesAsync(); // Save changes to the database
                // Call the broadcastMessage method to update clients with the new email
                await Clients.All.broadcastMessage(user.Email, message);
            }
            else
            {
                // Handle the case where the user is not found
                await Clients.All.ReceiveNotification($"User with ID {id} not found.");
            }
        }
        else
        {
            // Handle the case where the id is not a valid integer
            await Clients.All.ReceiveNotification($"Invalid user ID {id}.");
        }
    }

    private async Task<User> GetUserByConnectionId(string connectionId)
    {
        // Find the user by their connection ID
        return await _dbContext.users.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
    }
}
