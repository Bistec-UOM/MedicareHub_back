using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Text.Json; // Add this line
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Diagnostics;

namespace AppointmentNotificationHandler;
public interface IAppointmentNotificationClient
{
    Task ReceiveNotification(Notification notification);
    Task Receiver(string message);
    Task broadcastMessage(string name, string message);
    Task NotifyDoctor(int doctorId, string message);
    Task SendMessageToUser(string userId, string message);
}

public class AppointmentNotificationHub : Hub<IAppointmentNotificationClient>
{
    private readonly ApplicationDbContext _dbContext;





    public AppointmentNotificationHub(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }



    public override async Task OnConnectedAsync()
    {
        // Get the connection ID of the connected user
        var connectionId = Context.ConnectionId;
        // var token = Context.GetHttpContext().Request.Query["medicareHubToken"];
        var httpContext = Context.GetHttpContext();
        var userId = httpContext.Request.Query["userId"].ToString();

        ConnectionManager.AddConnection(userId, connectionId);

        Debug.WriteLine("User Connections:");
        foreach (var kvp in ConnectionManager._userConnections)
        {
            Debug.WriteLine($"User ID: {kvp.Key}, Connection ID: {kvp.Value}");
        }

        // Send a personalized message to the connected user
        //  await Clients.Client(connectionId).ReceiveNotification($"Hello kollone{userId}! Your Connection ID is: {connectionId}");

        await base.OnConnectedAsync();
    }


    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;
        var user = await _dbContext.users.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
        if (user != null)
        {
            await Clients.All.Receiver("User has disconnected.");
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
                //  await Clients.Client(Context.ConnectionId).ReceiveNotification($"User with ID {id} not found.");
            }
        }
        else
        {
            //  await Clients.Client(Context.ConnectionId).ReceiveNotification($"Invalid user ID {id}.");
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

    public async Task NotifyDoctor(int doctorId, string message)
    {
        Debug.WriteLine("User Connections:", ConnectionManager._userConnections);

        var doct = await _dbContext.doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
        var userId = doct.UserId;
        //var doctor = await _dbContext.users.FirstOrDefaultAsync(u => u.Id == doct.UserId);

        if (ConnectionManager._userConnections.TryGetValue(userId.ToString(), out string connectionId))
        {
            // Send the notification to the retrieved connection ID
            //  await Clients.Client(connectionId).ReceiveNotification(message);
        }



    }

    public async Task SendMessageToUser(string userId, string message)
    {
        if (ConnectionManager._userConnections.TryGetValue(userId, out string connectionId))
        {
            //  await Clients.Client(connectionId).ReceiveNotification(message);
        }
        else
        {
            // Handle case where user ID is not found in the dictionary
        }
    }



    //------------------------------------------------------message to pharmacist using analytics | admin related------------------------------------------------
    public async Task NotiToPharmacist()
    {

        var drugAvailability = await _dbContext.drugs
                       .Where(d => d.Avaliable < 10)
                       .Select(d => new
                       {
                           Name = d.GenericN + "(" + d.Weight + "mg)",
                           Available = d.Avaliable
                       })
                       .ToListAsync();

        var pharmacistConnections = await _dbContext.users
            .Where(u => u.Role == "Cashier" && u.ConnectionId != null)
            .Select(u => new
            {
                connectionId = u.ConnectionId,
                Id = u.Id
            })
            .ToListAsync();

        var unavailableDrugs = drugAvailability.Select(d => d.Name);
        string message = unavailableDrugs.Count() == 0
            ? ""
            : string.Join(", ", unavailableDrugs) + " drugs are less than 10 available";

        DateTime twentyFourHoursAgo = DateTime.Now.AddMinutes(330).AddHours(-24);
        bool messageExists = await _dbContext.notification
            .AnyAsync(n => n.Message == message && n.SendAt > twentyFourHoursAgo);
        Notification noti = new Notification();

        // noti.Message = message;
        // noti.SendAt = DateTime.Now;
        // noti.Seen = false;
        // noti.From = "System";
        // noti.To = 7.ToString();

        //List<Notification> notiList = new List<Notification>();
        if(message=="")
        {
            return;
        }
        else
        {
            foreach (var connection in pharmacistConnections)
            {
                noti.Message = message;
                noti.SendAt = DateTime.Now.AddMinutes(330);
                noti.Seen = false;
                noti.From = "system";
                noti.To = connection.Id.ToString();

                await _dbContext.notification.AddAsync(noti);
                if (noti.To != null && ConnectionManager._userConnections.TryGetValue(noti.To.ToString(), out var connectionId))
                {
                    await Clients.Client(connectionId).ReceiveNotification(noti);
                }
            }
        }
    }




}