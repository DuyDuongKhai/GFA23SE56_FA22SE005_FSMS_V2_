using FSMS.Entity.Models;
using Microsoft.AspNetCore.SignalR;

namespace FSMS.WebAPI.SignalRHubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userid"].ToString();
            if (userId is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string sender, string receiver, string message)
        {
            try
            {
                var context = new FruitSeasonManagementSystemV10Context();
                var time = DateTime.UtcNow.AddHours(7);
                context.ChatHistories.Add(new ChatHistory
                {
                    Sender = Int32.Parse(sender),
                    Receiver = Int32.Parse(receiver),
                    Message = message,
                    SendTimeOnUtc = time
                });

                await context.SaveChangesAsync();
                await Clients.Groups(sender, receiver).SendAsync("ReceiveMessage", sender, receiver, message, time);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}