using BeinConsoleAppStaj.Data;
using Microsoft.EntityFrameworkCore;

namespace BeinConsoleAppStaj.Entities;


public class DataContext : DbContext
{
    public string ConnectionString { get; }
    public DataContext()
    {
        ConnectionString = "Data Source=DESKTOP-KUOMH90\\SQLEXPRESS;Initial Catalog=Players;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";
    }
    public DbSet<FootballPlayer> FootballPlayers { get; set; }
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FootballPlayer>().HasKey(fp => fp.PlayerId);
    }

   
}
