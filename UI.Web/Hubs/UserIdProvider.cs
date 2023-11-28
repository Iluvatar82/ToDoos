using Microsoft.AspNetCore.SignalR;

namespace UI.Web.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var claim = connection.User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));
            return claim?.Value;
        }
    }
}
