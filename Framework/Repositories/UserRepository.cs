namespace Framework.Repositories
{
    public class UserRepository
    {
        public async Task<List<(string Name, string ListType, string Id)>> GetAllListsAsync(Guid userId)
        {
            var result = new List<(string Name, string ListType, string Id)>
            {
                ("Meine ToDo's", "person"/*people*/, "list/" + userId.ToString())
            };

            return result;
        }
    }
}
