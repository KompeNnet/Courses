using _5th_registration.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace _5th_registration
{
    [HubName("drawHub")]
    public class DrawHub : Hub
    {
        public void Send(LineData line)
        {
            Clients.AllExcept(Context.ConnectionId).addLine(line);
        }
    }
}