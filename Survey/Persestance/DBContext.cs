using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Survey.Abstractions;
using Survey.Models;
using System.Reflection;
using System.Security.Claims;

namespace Survey.Persestance
{
    public class DBContext(DbContextOptions<DBContext> options , IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Question> questions { get; set; }
        public DbSet<Poll> polls { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<Vote> votes { get; set; }
        public DbSet<VoteAnswers> voteAnswers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var CascadeFks = modelBuilder.Model.GetEntityTypes()
                .SelectMany(fk => fk.GetForeignKeys())
                .Where(Fk => Fk.DeleteBehavior == DeleteBehavior.Cascade && !Fk.IsOwnership);
            foreach (var fk in CascadeFks)
                fk.DeleteBehavior = DeleteBehavior.Restrict; 
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var Entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var EntriesEntry in Entries)
            {
                //get the current user ID
                var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
                //check on thier statues 
                if (EntriesEntry.State == EntityState.Added)
                {
                    EntriesEntry.Property(x => x.CreatedByID).CurrentValue = currentUserId!;
                }
                else if (EntriesEntry.State == EntityState.Modified)
                {
                    EntriesEntry.Property(x => x.UpdatedByID).CurrentValue = currentUserId!;
                    EntriesEntry.Property(x=>x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
