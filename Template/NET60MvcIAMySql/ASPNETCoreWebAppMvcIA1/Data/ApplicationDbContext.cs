// 注意: DbContext 和 database provider 為 1對1 的關係! 
// Each DbContext instance must be configured to use one and only one database provider. (Different instances of a DbContext subtype can be used with different database providers, but a single instance must only use one.) 
// ref: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#dbcontext-in-dependency-injection-for-aspnet-core

//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// add
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAppMvcIA1.Data
{
    //public class ApplicationDbContext : IdentityDbContext
    //{
    //    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    //        : base(options)
    //    {
    //    }
    //}
    // todo: 改成 public class ApplicationDbContext : ZDbContext 
    public class ApplicationDbContext: IdentityDbContext
    {   
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
            //base.OnConfiguring(optionsBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}