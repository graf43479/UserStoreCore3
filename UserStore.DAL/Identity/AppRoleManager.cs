﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.Owin;
//using Microsoft.Owin;
//using System.Data.Entity;
using System.Linq;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Identity
{
    public class AppRoleManager //: RoleManager<AppRole>
    {
     /*   public AppRoleManager(RoleStore<AppRole> store) : base(store)
        {            
        }

        public static AppRoleManager Create(
            IdentityFactoryOptions<AppRoleManager> options,
            IOwinContext context
            )
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<ApplicationContext>()));
        }

        public override IQueryable<AppRole> Roles => base.Roles.Include(x=>x.Users);*/
    }
}
