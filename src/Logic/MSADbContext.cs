using Microsoft.EntityFrameworkCore;
using SalaryLocator.Logic.Models;

namespace SalaryLocator.Logic
{
    public class MSADbContext : DbContext
    {
        public virtual DbSet<State> States { get; set; }

        public virtual DbSet<Area> Areas { get; set; }

        public virtual DbSet<Occupation> Occupations { get; set; }

        public virtual DbSet<Salary> Salaries { get; set; }

        public virtual DbSet<LivingWage> LivingWages { get; set; }

        public MSADbContext(DbContextOptions<MSADbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>(entity =>
            {
                entity.ForNpgsqlToTable("state");
                entity.HasKey(s => s.Code);
            });
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ForNpgsqlToTable("area");
                entity.HasKey(a => a.Code);
            });
            modelBuilder.Entity<Occupation>(entity =>
            {
                entity.ForNpgsqlToTable("occupation");
                entity.HasKey(a => a.Code);
            });
            modelBuilder.Entity<Salary>(entity =>
            {
                entity.ForNpgsqlToTable("salary");
                entity.HasKey(s => new { s.AreaCode, s.OccupationCode });
            });
            modelBuilder.Entity<LivingWage>(entity =>
            {
                entity.ForNpgsqlToTable("living_wage");
                entity.HasKey(lw => lw.Code);
            });
        }
    }
}
