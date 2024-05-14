using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    //public class MvcAppDbContext : DbContext
    //public class MvcAppDbContext : IdentityDbContext
    public class MvcAppDbContext : IdentityDbContext<ApplicationUser> //if you modify baseModel then need to make dbset from the modified user
    {
        public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options) : base(options)
        {
            //another easier way so send options=>(connection string) to onConfiguring in base() not my overload
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Server = DESKTOP-QVE29LM; Database = MvcApp ;Trusted_Connection= true; MultipleActiveResultSets=true ");//if i want muliple qurey from db
        //    optionsBuilder.UseSqlServer("Server = DESKTOP-QVE29LM; Database = MvcApp ;Trusted_Connection= true;");
        //}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }



        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }

        //instead of make dbset of those you can inherts from Identitydbcontext it make those 2 dbset 
        //and also inheret dbContext so you As inherts from dbcontext normal
        //name of those dbset is users,roles and thier tables will be aspNetUser,aspNetRole

        //must add migration as you add 2 dbsets but you inherts them from IdetitiyDbContext





    }
}
