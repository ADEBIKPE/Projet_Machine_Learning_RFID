using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projet_Tech_Pag_Con.Data;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Projet_Tech_Pag_Con.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurer les services d'authentification et autres services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events = new CookieAuthenticationEvents
        {
            OnSignedIn = async context =>
            {
                var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                var user = await userManager.GetUserAsync(context.Principal);

                if (user != null && await userManager.IsInRoleAsync(user, "2"))
                {
                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

                    var simulation = new Simulation
                    {
                        UtilisateurId = user.Id,
                        DateSimulation = DateTime.Now
                    };

                    dbContext.Simulation.Add(simulation);
                    await dbContext.SaveChangesAsync();
                }
            }
        };
    });

builder.Services.AddDbContext<Projet_Tech_Pag_ConContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Projet_Tech_Pag_ConContextConnection") ?? throw new
    InvalidOperationException("Connection string 'Projet_Tech_Pag_ConContextConnection' not found.")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Projet_Tech_Pag_ConContextConnection") ?? throw new
    InvalidOperationException("Connection string 'Projet_Tech_Pag_ConContextConnection' not found.")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Configurer la session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedDataSimu.Initialize(services);
    SeedDataExecutMeth.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Utiliser la session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
