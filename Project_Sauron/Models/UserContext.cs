using System.Data.Entity;

namespace Project_Sauron.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DefaultConnection")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<EnemyEvent> EnemyEvents { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<SiteTheme> SiteThemes { get; set; }
    }
}