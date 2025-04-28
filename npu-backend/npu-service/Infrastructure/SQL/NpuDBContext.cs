using Domain.Npu;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL;

public class NpuDBContext : DbContext
{
    public NpuDBContext(DbContextOptions<NpuDBContext> options) : base(options) { }

    public DbSet<Npu> Npus { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Npu>(entity =>
        {
            entity.ToTable("Npus");
            entity.HasKey(e => e.ID);

            entity.Property(e => e.ID)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(e => e.ElementName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.PictureID)
                .IsRequired();

            entity.Property(e => e.CreatedBy)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}