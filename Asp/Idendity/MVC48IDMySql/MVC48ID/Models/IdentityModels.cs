//using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
// add
using System.Configuration;
using LibIdentityMySql.DIdentity;
using ZLib.DLib;

namespace MVC48ID.Models
{
    // 將 IdentityModel.cs 中 ApplicationUser類別改為繼承 LibSQL.DIdentity.User
    //public class ApplicationUser : IdentityUser

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : User
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    // 將 ApplicationDbContext 類別改為使用 ZMySqlClient 公用程式, 並傳入ConnectionString
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    public class ApplicationDbContext : ZMySqlClient
    {
        public ApplicationDbContext()
            // ZMySqlClient 建構需傳入 ConnectionString.
            // 執行前, 記得要修改 Web.config 中 DefaultConnection 的連線字串.
            //: base("DefaultConnection", throwIfV1Schema: false)
            : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}