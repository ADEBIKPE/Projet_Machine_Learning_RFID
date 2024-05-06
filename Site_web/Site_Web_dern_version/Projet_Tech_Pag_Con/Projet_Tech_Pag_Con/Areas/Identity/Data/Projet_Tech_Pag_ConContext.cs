using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Projet_Tech_Pag_Con.Data;

public class Projet_Tech_Pag_ConContext : IdentityDbContext<IdentityUser>
{
    public Projet_Tech_Pag_ConContext(DbContextOptions<Projet_Tech_Pag_ConContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
