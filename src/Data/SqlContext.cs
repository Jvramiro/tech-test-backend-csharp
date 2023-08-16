using Microsoft.EntityFrameworkCore;
using TestJrAPI.Models;

namespace TestJrAPI.Data {
    public class SqlContext : DbContext {

        public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Produto>()
                .Property(p => p.Nome).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco).HasColumnType("decimal(10,2)").IsRequired(true);

        }

    }
}
