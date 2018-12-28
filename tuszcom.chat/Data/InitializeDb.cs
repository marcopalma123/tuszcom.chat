using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using tuszcom.chat.Models;

namespace tuszcom.chat.Data
{
    public class InitializeDb
    {
        public async Task Initialize(ChatDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();
           await FirstConfigUserRoles(userManager, roleManager, context);
        }

        private async Task CreateRole(string roleName, RoleManager<ApplicationRole> roleManager, ChatDbContext context)
        {

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();
            if (!roleExists.Result)
            {
                Task<IdentityResult> roleResult = roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                roleResult.Wait();
            }

            if (roleName == "SuperAdmin")
            {
                var SuperAdminRole = roleManager.FindByNameAsync("SuperAdmin");
                if (SuperAdminRole.Result != null)
                {            
                    var claims = context.RoleClaims.Where(x => x.RoleId == SuperAdminRole.Result.Id.ToString()).ToList();
                    foreach (var claim in ClaimsData.Claims)
                    {
                        if (!claims.Exists(x => x.ClaimValue == claim))
                           await roleManager.AddClaimAsync(SuperAdminRole.Result, new Claim(claim, claim));
                    }
                }
            }
        }

        public async Task AddUser(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
           var a =  await userManager.CreateAsync(user, "Admin123!");
        }

        public async Task AddRoleToUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string RoleName)
        {
            await userManager.AddToRoleAsync(user, RoleName);
        }

        public async Task FirstConfigUserRoles(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, ChatDbContext context)
        {
            string[] roleNames = { "SuperAdmin", "Administrator", "User" };


            foreach (string roleName in roleNames)
            {
               await CreateRole(roleName, roleManager, context);
            }

            var userExists = userManager.FindByEmailAsync("jakub.tuszynski1992@gmail.pl");
            userExists.Wait();


            if (userExists.Result == null)
            {
                var AdministratorRole = roleManager.FindByNameAsync("SuperAdmin");
                ApplicationUser user = new ApplicationUser
                {
                    Email = "jakub.tuszynski1992@gmail.pl",
                    UserName = "TuszcomAdmin",
                    EmailConfirmed = true,
                    FirstName = "Jakub",
                    SurName = "Tuszyński"

                };

                await AddUser(userManager, user);
                await AddRoleToUser(userManager, user, AdministratorRole.Result.Name);
            }
        }
    }
}

