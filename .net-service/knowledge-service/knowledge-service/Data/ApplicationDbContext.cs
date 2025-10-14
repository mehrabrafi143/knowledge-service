namespace knowledge_service.Data
{
    using knowledge_service.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<KnowledgeEntry> KnowledgeEntries => Set<KnowledgeEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();

                // SQL Server specific conversion for string list
                entity.Property(e => e.Tags)
                      .HasConversion(
                          v => string.Join(';', v),
                          v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                      );

                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.CreatedAt);
            });

            // SQL Server specific table name
            modelBuilder.Entity<KnowledgeEntry>().ToTable("KnowledgeEntries");
        }
    }
}
