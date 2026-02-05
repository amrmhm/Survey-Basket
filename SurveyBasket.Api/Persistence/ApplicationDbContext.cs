
using Microsoft.Identity.Client;
using System.Reflection;
using System.Security.Claims;

namespace SurveyBasket.Api.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor)
    :IdentityDbContext<ApplicationUser, ApplicationRole,string>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Poll> Polls { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<VoteAnswer> VoteAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Add Defalt Restrict To On Delete Methods
       var cascadeFks = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
        foreach (var fk in cascadeFks)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
        //Add Default All Configrations 
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserCLaim");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
    
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {

        var entries = ChangeTracker.Entries<AuditableEntity>();
        //this Extentions Methods to Return UserId from Authorize used http context
        var currentUseId = _httpContextAccessor.HttpContext?.User.GetUserId();
        //or
        //var currentUseId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        foreach (var entryEntities in entries)
        {
            if(entryEntities.State == EntityState.Added)
            {
                entryEntities.Property(c => c.CreateById).CurrentValue = currentUseId!;
            }
            else if(entryEntities.State == EntityState.Modified)
            {
                entryEntities.Property(c => c.UpdateById).CurrentValue = currentUseId;
                entryEntities.Property(c => c.UpdateOn).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

}
