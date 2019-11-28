using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.EF
{
    public class DBInitializer
    {
        //private ApplicationContext _context;
        //private RoleManager<AppRole> _roleManager;
        //private UserManager<AppUser> _userManager;


        //public DBInitializer(ApplicationContext context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        //{
        //    _roleManager = roleManager;
        //    _userManager = userManager;
        //    _context = context;

        //}

        //public async Task InitializeAsync()
        //{
        //    //RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>();
        //    //UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>();

        //    _context.Database.EnsureCreated();

        //    // Look for any students.
        //    if (!_context.Users.Any())
        //    {
        //        //create user and admin role

        //        AppUser adminUser = new AppUser();

        //        adminUser.Email = "admin@company.com";
        //        adminUser.UserName = "Admin";

        //        var result = await _userManager.CreateAsync(adminUser, "Password-1");

        //        var newAdminUser = await _userManager.FindByEmailAsync(adminUser.Email);

        //        AppRole adminRole = new AppRole();

        //        adminRole.Name = "Admin";
        //        //adminRole.Description = "This is the admin role.";

        //        await _roleManager.CreateAsync(adminRole);

        //        await _roleManager.AddClaimAsync(adminRole, new Claim("Can add roles", "add.role"));
        //        await _roleManager.AddClaimAsync(adminRole, new Claim("Can delete roles", "delete.role"));
        //        await _roleManager.AddClaimAsync(adminRole, new Claim("Can edit roles", "edit.role"));

        //        await _userManager.AddToRoleAsync(newAdminUser, adminRole.Name);

        //        //create user and basic role

        //        AppUser basicUser = new AppUser();

        //        basicUser.Email = "basic@company.com";
        //        basicUser.UserName = "Basic";

        //        var resultBasic = await _userManager.CreateAsync(basicUser, "Password-1");

        //        var newBasicUser = await _userManager.FindByEmailAsync(basicUser.Email);

        //        AppRole basicRole = new AppRole();

        //        basicRole.Name = "Basic";
        //        //basicRole.Description = "This is the basic role.";

        //        await _roleManager.CreateAsync(basicRole);

        //        //await _roleManager.AddClaimAsync(basicRole, new Claim("Can add roles", "add.role"));
        //        //await _roleManager.AddClaimAsync(basicRole, new Claim("Can delete roles", "delete.role"));
        //        //await _roleManager.AddClaimAsync(basicRole, new Claim("Can edit roles", "edit.role"));

        //        await _userManager.AddToRoleAsync(newBasicUser, basicRole.Name);

        //        await _context.SaveChangesAsync();
        //    }

        //}


        //public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        //{


        //    string adminEmail = "admin@gmail.com";
        //    string password = "_Aa123456";
        //    if (roleManager.FindByNameAsync("admin") == null)
        //    {
        //        await roleManager.CreateAsync(new AppRole("admin"));
        //    }


        //    if (roleManager.FindByNameAsync("user") == null)
        //    {
        //        await roleManager.CreateAsync(new AppRole("user"));
        //    }


        //    if (await userManager.FindByNameAsync(adminEmail) == null)
        //    {
        //        AppUser admin = new AppUser { Email = adminEmail, UserName = adminEmail };
        //        IdentityResult result = await userManager.CreateAsync(admin, password);
        //        if (result.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(admin, "admin");
        //        }
        //    }
        //}

        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {


            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";

            //IdentityResult res0 =  await roleManager.CreateAsync(new AppRole("SuperAdmin"));

            //  if (res0.Succeeded)
            //  {
            //      string s = "";
            //  }
            //  else
            //  {
            //      string s = res0.Errors.First().Description;
            //  }
            //AppRole role = await roleManager.FindByNameAsync("admin");
            //if (role == null)
            //{ 
            //    IdentityResult res = await roleManager.CreateAsync(new AppRole("admin"));
            //    if (res.Succeeded)
            //    {
            //        string message = "Good";
            //    }
            //    else
            //    {
            //         string  message = res.Errors.First().Description;
            //    }

            //}

            string[] roles = { "admin", "user" };
            foreach (string role  in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    if ((await roleManager.CreateAsync(new IdentityRole(role))).Succeeded != true)
                    {
                        throw new Exception($"Ошибка при создании роли \'{role}\'");
                    }
                }
            }


         

            //if (roleManager.FindByNameAsync("admin") == null)
            //{

            //    await roleManager.CreateAsync(new AppRole("admin"));
            //}


            //if (roleManager.FindByNameAsync("user") == null)
            //{
            //    await roleManager.CreateAsync(new AppRole("user"));
            //}


            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                AppUser admin = new AppUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }

        //public static void Initialize(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        //{


        //    string adminEmail = "admin@gmail.com";
        //    string password = "_Aa123456";
        //    if (roleManager.FindByNameAsync("admin") == null)
        //    {
        //        roleManager.CreateAsync(new AppRole("admin"));
        //    }


        //    if (roleManager.FindByNameAsync("user") == null)
        //    {
        //        roleManager.CreateAsync(new AppRole("user"));
        //    }


        //    if (userManager.FindByNameAsync(adminEmail) == null)
        //    {
        //        AppUser admin = new AppUser { Email = adminEmail, UserName = adminEmail };
        //        //IdentityResult result = await userManager.CreateAsync(admin, password);
        //        userManager.CreateAsync(admin, password);
        //        //if (result.Succeeded)
        //        //{
        //            userManager.AddToRoleAsync(admin, "admin");
        //        //}
        //    }
        //}

    }
}
