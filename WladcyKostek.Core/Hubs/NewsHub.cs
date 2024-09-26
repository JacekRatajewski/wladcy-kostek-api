using Microsoft.AspNetCore.SignalR;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;

namespace WladcyKostek.Core.Hubs
{
    public class NewsHub : Hub
    {
        public async Task SendMessage(NewsDTO news)
        {
            await Clients.All.SendAsync("ReceiveMessage", news);
        }
    }
}
