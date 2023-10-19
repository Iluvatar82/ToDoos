using Microsoft.AspNetCore.SignalR;

namespace UI.Web.Hubs
{
    public class GroupListUpdateHub : Hub
    {
        public const string HubUrl = "/hubs/updatelist";

        public async Task JoinedGroupList(Guid listId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, listId.ToString());
        }

        public async Task LeftGroupList(Guid listId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, listId.ToString());
        }

        public async Task BroadcastListChanged(Guid listId)
        {
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("UpdateList", Context.User?.Identity?.Name);
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
