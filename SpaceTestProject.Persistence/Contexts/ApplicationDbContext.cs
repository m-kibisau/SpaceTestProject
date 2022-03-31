using System;
using Microsoft.EntityFrameworkCore;
using SpaceTestProject.Domain;

namespace SpaceTestProject.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<WatchListItem> WatchListItems { get; set; }
        public virtual DbSet<WatchListEmailLog> WatchListEmailLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WatchListItem>(b =>
            {
                b.HasKey(x => x.Id);
            });

            builder.Entity<WatchListEmailLog>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.WatchList)
                    .WithMany()
                    .HasForeignKey(x => x.WatchListId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
