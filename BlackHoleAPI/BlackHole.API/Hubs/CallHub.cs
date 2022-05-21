using BlackHole.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlackHole.API.Hubs
{
    public class CallHub : Hub
    {
        //public async Task Call(string user)
        //{
        //    await Clients.All.SendAsync(Constants.ReceiveHubMessageMethod, user, text);
        //}
    }
}
