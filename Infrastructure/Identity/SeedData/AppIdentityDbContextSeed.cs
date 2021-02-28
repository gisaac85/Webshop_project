using Core.Entities.UserModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.SeedData
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Isaac",
                    Email = "isaac@webshop.nl",
                    UserName = "isaac@webshop.nl",
                    AddressUser = new AddressUser
                    {
                        FirstName = "Gaorieh",
                        LastName = "Isaac",
                        Street = "Deimtstraat 26",
                        City = "Purmerend",
                        State = "Noord Holland",
                        Zipcode = "1445GN"
                    }
                };

                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}
