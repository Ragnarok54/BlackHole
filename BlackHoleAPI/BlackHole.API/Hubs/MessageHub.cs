using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlackHole.API.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //await Clients.SendAsync("ReceiveMessage", user, message);
        }
    }
}
