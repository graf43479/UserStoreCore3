using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Identity;
using UserStore.DAL.Interfaces;

namespace UserStore.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        //private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<AppRole> _roleManager;
        private ClientRepository clientRepository;


        private ProductRepository productRepository;
        private ExceptionRepository exceptionRepository;

        public IdentityUnitOfWork(DbContextOptions<ApplicationContext> options)
        {            
            //db = new ApplicationContext(connectionString);
            db = new ApplicationContext(options);
           // _userManager = new UserManager<AppUser>(new UserStore<AppUser>(db),);// AppUserManager(new UserStore<AppUser>(db));          
          // _roleManager = new AppRoleManager(new RoleStore<AppRole>(db));
            clientRepository = new ClientRepository(db);
            productRepository = new ProductRepository(db);
            exceptionRepository = new ExceptionRepository(db);
        }

       // public AppUserManager UserManager => userManager;

        public IRepository<ClientProfile> Clients
        {
            get
            {
                if (clientRepository == null)
                    clientRepository = new ClientRepository(db);
                return clientRepository;
            }
        }
    
       // public AppRoleManager RoleManager => roleManager;

        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
                return productRepository;
            }
        }

        public IRepository<ExceptionDetail> ExceptionDetails
        {
            get
            {
                if (exceptionRepository == null)
                    exceptionRepository = new ExceptionRepository(db);
                return exceptionRepository;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }


        //public void Dispose()
        //{
        //    db.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //private bool disposed = false;

        //public virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            userManager.Dispose();
        //            roleManager.Dispose();
        //            clientRepository.Dispose();
        //        }
        //        disposed = true;
        //    }
        //}        
    }
}
