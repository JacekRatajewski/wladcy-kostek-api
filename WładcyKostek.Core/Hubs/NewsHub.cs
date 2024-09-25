using Microsoft.AspNetCore.SignalR;
using WładcyKostek.Core.Interfaces;
using WładcyKostek.Core.Models;

namespace WładcyKostek.Core.Hubs
{
    public class NewsHub : Hub
    {
        public async Task SendMessage(NewsDTO news)
        {
            await Clients.All.SendAsync("ReceiveMessage", news);
        }
    }
}
