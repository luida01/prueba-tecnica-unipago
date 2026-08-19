using Microsoft.EntityFrameworkCore;
using PersonaStatsApi.Models;

namespace PersonaStatsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<SocialStats> SocialStats => Set<SocialStats>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SocialStats>().HasData(

            new SocialStats { Id = 1, Name = "Conocimiento", Level = 1, Points = 0},
            new SocialStats { Id = 2, Name = "Coraje", Level = 1, Points = 0},
            new SocialStats { Id = 3, Name = "Amabilidad", Level = 1, Points = 0},
            new SocialStats { Id = 4, Name = "Proeza", Level = 1, Points = 0},
            new SocialStats { Id = 5, Name = "Valentia", Level = 1, Points = 0}
            
        );
        
    }

}
