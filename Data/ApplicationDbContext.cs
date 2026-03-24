using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo_list.Models; // Để nhận diện được TodoTask
// ...
public class ApplicationDbContext : IdentityDbContext // Đổi ở đây
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Todo_list.Models.TodoTask> TodoTasks { get; set; }
}