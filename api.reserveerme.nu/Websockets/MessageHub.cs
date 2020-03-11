using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace api.reserveerme.nu.Websockets
{
    public class MessageHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
    }
}