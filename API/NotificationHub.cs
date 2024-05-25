using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

{
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        {
            }

        }

            {
        }

        public static string GetConnectionId(string userId)
        {
            UserConnections.TryGetValue(userId, out var connectionId);
            return connectionId;
        }
    }
}
