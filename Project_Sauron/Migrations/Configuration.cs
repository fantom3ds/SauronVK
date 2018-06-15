namespace Project_Sauron.Migrations
{
    using Project_Sauron.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Project_Sauron.Models.UserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Project_Sauron.Models.UserContext DB)
        {
            DB.Roles.Add(new Role { Id = 1, Name = "admin" });
            DB.Roles.Add(new Role { Id = 2, Name = "user" });
            DB.SaveChanges();
            DB.Users.Add(new User { Id = 1, Login = "Admin", Password = EncoderGuid.PasswordToGuid.Get("2033"), RoleId = 1 });
            DB.SaveChanges();
            base.Seed(DB);
        }
    }
}
