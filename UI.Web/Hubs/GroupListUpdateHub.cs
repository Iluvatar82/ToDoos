using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace UI.Web.Hubs
{
    public class GroupListUpdateHub : Hub
    {
        public const string HubUrl = "/hubs/updatelist";

        public async Task JoinedGroupList(Guid listId, ClaimsPrincipal? user = null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, listId.ToString());
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("JoinedList", user?.Identity?.Name);
        }

        public async Task LeftGroupList(Guid listId, ClaimsPrincipal? user = null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, listId.ToString());
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("LeftList", user?.Identity?.Name);
        }

        public async Task BroadcastListChanged(Guid listId, ClaimsPrincipal? user = null)
        {
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("UpdateList", user?.Identity?.Name);
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
