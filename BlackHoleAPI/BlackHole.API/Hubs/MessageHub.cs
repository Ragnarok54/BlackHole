using BlackHole.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlackHole.API.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string text)
        {
            await Clients.All.SendAsync(Constants.ReceiveHubMessageMethod, user, text);
        }
    }
}
