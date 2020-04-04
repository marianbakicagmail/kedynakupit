using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
public partial class KedyNakupitContext : DbContext
{
    public KedyNakupitContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var cstr = config.GetConnectionString("KedyNakupitDbConnectionString");

            optionsBuilder.UseSqlServer(cstr);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CashRegisterRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.Property(e => e.RecordId)
                    .HasColumnName("RecordId")
                    .ValueGeneratedNever();

                entity.Property(e => e.TimeStamp)
                    .IsRequired();

                entity.Property(e => e.LegalEntityIdentifier)
               .IsRequired();

                entity.Property(e => e.Address)
               .IsRequired();

                entity.Property(e => e.TransactionCount)
               .IsRequired();
            });
    }

    public virtual DbSet<CashRegisterRecord> CashRegisterRecord { get; set; }

}
