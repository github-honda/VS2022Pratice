using Microsoft.AspNetCore.Identity;
namespace NET60MvcIAMySql.Data
{
    public class IdentityUserMySql: IdentityUser
    {
        public IdentityUserMySql()
        {
            Id = Guid.NewGuid().ToString();

        }
    }
}
