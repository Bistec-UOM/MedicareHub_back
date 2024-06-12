using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Text.Json; // Add this line
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Connections.Features;
using Sprache;
using Services.PharmacyService;


public interface INotificationClient
{
    Task ReceiveNotification(string message);
    Task Receiver(string message);
    Task Render(List<Drug> medicinesNotInStock);
    Task broadcastMessage(string name, string message);
    Task ASking(List<string> medicineNames);


}
public class NotificationHub : Hub<INotificationClient>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly BillService _billService;

    public NotificationHub(ApplicationDbContext dbContext, BillService billService)
    {
        _dbContext = dbContext;
        _billService = billService;
    }

    public override async Task OnConnectedAsync()
    {
        //await Clients.All.ReceiveNotification($"Thank you for connecting: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;
        var user = await _dbContext.users.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
        if (user != null)
        {
            user.IsActive = false;
            user.ConnectionId = null;
            await _dbContext.SaveChangesAsync();
            await Clients.All.broadcastMessage(user.Name, "User has manually disconnected.");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task ManualDisconnect(string Id)
    {
        if (int.TryParse(Id, out int userId))
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.IsActive = false;
                user.ConnectionId = null;
                await _dbContext.SaveChangesAsync();
                await Clients.All.broadcastMessage(user.Name, "User has manually disconnected.");
            }
        }
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
                user.IsActive = true;
                user.ConnectionId = Context.ConnectionId;
                await _dbContext.SaveChangesAsync();
                await Clients.Client(Context.ConnectionId).broadcastMessage(user.Name, message);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"User with ID {id} not found.");
            }
        }
        else
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"Invalid user ID {id}.");
        }
    }

    public async Task UsersCalling()
    {
        await UpdateUserList();
    }

    private async Task UpdateUserList()
    {
        var users = await _dbContext.users.ToListAsync();
        var usersJson = JsonSerializer.Serialize(users);
        await Clients.All.Receiver(usersJson);
    }

    public async Task NotiToPharmacist()
    {
        var drugAvailability = _dbContext.drugs
            .Where(d => d.Avaliable < 10)
            .Select(d => new
            {
                Name = d.BrandN + "(" + d.Weight + "mg)",
                Available = d.Avaliable
            })
            .ToList();

        var pharmacistConnections = _dbContext.users
            .Where(u => u.Role == "Cashier" && u.ConnectionId != null)
            .Select(u => new
            {
                connectionId = u.ConnectionId,
                Id = u.Id
            })
            .ToList();

        var unavailableDrugs = drugAvailability.Select(d => d.Name);
        var message = string.Join(", ", unavailableDrugs) + " drugs are less than 10 available";

        bool messageExists = await _dbContext.notification.AnyAsync(n => n.Message == message);

        if (!messageExists)
        {
            var notifications = new List<Notification>();

            foreach (var connection in pharmacistConnections)
            {
                await Clients.Client(connection.connectionId).ReceiveNotification(message); 

                var notification = new Notification
                {
                    From = "System",
                    To = connection.Id.ToString(),
                    Message = message,
                    SendAt = DateTime.Now,
                    Seen = false
                };

                notifications.Add(notification);
            }

            await _dbContext.notification.AddRangeAsync(notifications);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task ASking(List<string> medicineNames)
    {
        var medicinesNotInStock = await _billService.GetMedicinesNotInStock(medicineNames);
        //await Clients.All.Render(medicinesNotInStock); // Ensure this line is active to broadcast the data
    }
}
