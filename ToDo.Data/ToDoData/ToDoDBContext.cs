using Microsoft.EntityFrameworkCore;
using ToDo.Data.ToDoData.Entities;

namespace ToDo.Data.ToDoData
{
    public class ToDoDBContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<UserGroup> Groups { get; set; }
        public DbSet<Setting> Settings { get; set; }


        public ToDoDBContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
    }
}
