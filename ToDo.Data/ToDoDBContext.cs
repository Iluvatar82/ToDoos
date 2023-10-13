using ToDo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data
{
    public class ToDoDBContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Category> Categories { get; set; }


        public ToDoDBContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
    }
}
