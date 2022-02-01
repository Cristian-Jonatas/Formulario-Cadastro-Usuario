using Microsoft.EntityFrameworkCore;

namespace CadastroUsuario
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(u => {
                u.HasKey(u => u.Id);
                u.Property(u => u.Id).ValueGeneratedOnAdd().HasColumnType("int");

                u.Property(u => u.Nome).HasMaxLength(250).IsRequired();
                u.Property(u => u.Email).HasMaxLength(250).IsRequired();
                u.Property(u => u.CPF).HasMaxLength(20).IsRequired();
                u.Property(u => u.DataNascimento).IsRequired();
            });

            modelBuilder.Entity<Endereco>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnType("int");
                e.Property(e => e.CEP).HasMaxLength(20).IsRequired();
                e.Property(e => e.Logradouro).HasMaxLength(1000).IsRequired();
                e.Property(e => e.Complemento).HasMaxLength(1000).IsRequired();
                e.Property(e => e.Bairro).HasMaxLength(250).IsRequired();
                e.Property(e => e.Cidade).HasMaxLength(250).IsRequired();
                e.Property(e => e.Estado).HasMaxLength(20).IsRequired();
                e.Property(e => e.IdUsuario).HasColumnType("int").IsRequired();
            });
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
    }
}
