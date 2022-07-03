using BlackHole.Business.Services;
using BlackHole.Common;
using BlackHole.Common.Enums;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackHole.API.Hubs
{
    public class StatusHub : Hub
    {
        private readonly static HashSet<Guid> activeUserIds = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            activeUserIds.Add(new Guid(userId));

            await Clients.User(userId).SendAsync(Constants.StatusHubAllActive, activeUserIds);
            await Clients.AllExcept(userId).SendAsync(Constants.StatusActiveHubMethod, userId);
        }

        public async Task Reject(string userId)
        {
            await Clients.User(userId).SendAsync(Constants.CallRejected);
        }

        public async Task Close(string userId)
        {
            await Clients.User(userId).SendAsync(Constants.CallClosed);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;

            activeUserIds.Remove(new Guid(userId));

            await Clients.All.SendAsync(Constants.StatusInactiveHubMethod, userId);
        }
    }
}
