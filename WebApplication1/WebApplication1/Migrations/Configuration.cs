namespace WebApplication1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication1.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApplication1.Models.ApplicationDbContext context)
        {
            
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin")) 
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "rgoodwin16@outlook.com"))
            {
                userManager.Create(new ApplicationUser
                    {
                        UserName = "rgoodwin16@outlook.com",
                        Email = "rgoodwin16@outlook.com",
                        FirstName = "Ray",
                        LastName = "Goodwin",
                        DisplayName = "Nanye East"
                    }, "leeray1");
            }

            var userId = userManager.FindByEmail("rgoodwin16@outlook.com").Id;
            userManager.AddToRole(userId, "Admin");

            var coderRole = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                coderRole.Create(new IdentityRole { Name = "Moderator" });
            }

            var codeManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                codeManager.Create(new ApplicationUser
                    {
                        UserName = "moderator@coderfoundry.com",
                        Email = "moderator@coderfoundry.com",
                        DisplayName = "Coderfoundry Moderator"
                    }, "Password-1");
                    
            }

            var 
                coderId = codeManager.FindByEmail("moderator@coderfoundry.com").Id;
            codeManager.AddToRole(coderId, "Moderator");

        }
    }
}
