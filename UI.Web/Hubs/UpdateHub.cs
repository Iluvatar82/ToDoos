using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace UI.Web.Hubs
{
    public class UpdateHub : Hub, IDisposable
    {
        public readonly static UserConnectionMapper Connections = new UserConnectionMapper();
        public const string HubUrl = "/hubs/update";

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

        public async Task UpdateNotifications(List<string> userIds)
        {
            var clients = Clients.Clients(Connections.GetConnections(userIds));
            await clients.SendAsync("UpdateNotifications");
        }

        public void RegisterId(string userId)
        {
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
                Connections.Add(userId, Context.ConnectionId);
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            string name = Context.UserIdentifier;
            Connections.Remove(name, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
