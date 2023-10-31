using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace UI.Web.Hubs
{
    public class GroupListUpdateHub : Hub
    {
        public const string HubUrl = "/hubs/updatelist";

        public async Task JoinedGroupList(Guid listId, IdentityUser user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, listId.ToString());
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("JoinedList", user.UserName);
        }

        public async Task LeftGroupList(Guid listId, IdentityUser user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, listId.ToString());
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("LeftList", user.UserName);
        }

        public async Task BroadcastListChanged(Guid listId, IdentityUser user)
        {
            await Clients.GroupExcept(listId.ToString(), Context.ConnectionId).SendAsync("UpdateList", user.UserName);
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
