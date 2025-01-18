namespace ContactManager.Migrations
{
    using System.Data.Entity.Migrations;
    using ContactManager.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ContactManager.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        bool AddUserAndRole(ContactManager.Models.ApplicationDbContext context)
        {
            #region Not detailed
            //IdentityResult ir;
            //var rm = new RoleManager<IdentityRole>
            //    (new RoleStore<IdentityRole>(context));
            //ir = rm.Create(new IdentityRole("canEdit"));
            //var um = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(context));
            //var user = new ApplicationUser()
            //{
            //    UserName = "user1@contoso.com",
            //};
            //ir = um.Create(user, "P_assw0rd1");
            //if (ir.Succeeded == false)
            //    return ir.Succeeded;
            //ir = um.AddToRole(user.Id, "canEdit");
            //return ir.Succeeded; 
            #endregion

            IdentityResult identityResult = new IdentityResult();

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);
            IdentityRole canEditIdentityRole = new IdentityRole("canEdit");

            identityResult = roleManager.Create(canEditIdentityRole);

            if (identityResult.Succeeded == false)
                return identityResult.Succeeded;

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = "abubaker_ms1@hotmail.com";

            identityResult = userManager.Create(applicationUser, "P_assw0rd1");

            if (identityResult.Succeeded == false)
                return identityResult.Succeeded;

            identityResult = userManager.AddToRole(applicationUser.Id, canEditIdentityRole.Name);
            return identityResult.Succeeded;
        }
    
        protected override void Seed(ContactManager.Models.ApplicationDbContext context)
        {
            AddUserAndRole(context);

            context.Contacts.AddOrUpdate(p => p.Name,
               new Contact
               {
                   Name = "Debra Garcia",
                   Address = "1234 Main St",
                   City = "Redmond",
                   State = "WA",
                   Zip = "10999",
                   Email = "debra@example.com",
               },
                new Contact
                {
                    Name = "Thorsten Weinrich",
                    Address = "5678 1st Ave W",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "thorsten@example.com",
                },
                new Contact
                {
                    Name = "Yuhong Li",
                    Address = "9012 State st",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "yuhong@example.com",
                },
                new Contact
                {
                    Name = "Jon Orton",
                    Address = "3456 Maple St",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "jon@example.com",
                },
                new Contact
                {
                    Name = "Diliana Alexieva-Bosseva",
                    Address = "7890 2nd Ave E",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "diliana@example.com",
                }
                );
        }
       }
}
