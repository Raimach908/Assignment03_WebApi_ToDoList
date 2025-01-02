using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Models;

namespace ToDoApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-many relationship
            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.User) // A TodoItem has one User
                .WithMany(u => u.TodoItems) // A User has many TodoItems
                .HasForeignKey(t => t.UserId) // Foreign key in TodoItem
                .OnDelete(DeleteBehavior.Cascade); // Cascading delete if a user is deleted

            base.OnModelCreating(modelBuilder);
        }
    }
}
