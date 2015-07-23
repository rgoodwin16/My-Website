namespace WebApplication1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication1.Models;
    using System.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApplication1.Models.ApplicationDbContext context)
        {
            //Create Admin Role
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin")) 
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var adminEmail = ConfigurationManager.AppSettings["AdminEmail"];

            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                userManager.Create(new ApplicationUser
                    {
                        UserName = ConfigurationManager.AppSettings["AdminEmail"],
                        Email = ConfigurationManager.AppSettings["AdminEmail"],
                        FirstName = ConfigurationManager.AppSettings["AdminFirstName"],
                        LastName = ConfigurationManager.AppSettings["AdminLastName"],
                        DisplayName = ConfigurationManager.AppSettings["AdminDisplayName"]
                    }, ConfigurationManager.AppSettings["AdminPassword"]);
            }

            var userId = userManager.FindByEmail(adminEmail).Id;
            userManager.AddToRole(userId, "Admin");

            //Create Moderator Role
            var coderRole = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                coderRole.Create(new IdentityRole { Name = "Moderator" });
            }

            var codeManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var moderatorEmail = ConfigurationManager.AppSettings["ModeratorEmail"];

            if (!context.Users.Any(u => u.Email == moderatorEmail))
            {
                codeManager.Create(new ApplicationUser
                    {
                        UserName = ConfigurationManager.AppSettings["ModeratorEmail"],
                        Email = ConfigurationManager.AppSettings["ModeratorEmail"],
                        DisplayName = ConfigurationManager.AppSettings["ModeratorDisplayName"]
                    }, ConfigurationManager.AppSettings["ModeratorPassword"]);
                    
            }

            var coderId = codeManager.FindByEmail(moderatorEmail).Id;
            codeManager.AddToRole(coderId, "Moderator");

            

        }
    }
}
