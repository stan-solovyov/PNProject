using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace PriceNotifier.Hubs
{
    public class PriceHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
        }
    }
}