using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace UserStore.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
