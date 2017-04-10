using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Context
{
    public class ExampleDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) 
            : base(options)
        {

        }
    }
}
