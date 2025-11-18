using Microsoft.EntityFrameworkCore;
using GenFitNet.Infrastructure.Models;

namespace GenFitNet.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Candidato> Candidatos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Vaga
        modelBuilder.Entity<Vaga>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Requisitos).HasMaxLength(2000);
            entity.Property(e => e.Localizacao).HasMaxLength(200);
            entity.Property(e => e.TipoContrato).HasMaxLength(50);
            entity.Property(e => e.DataCriacao).IsRequired();
        });

        // Configuração da entidade Candidato
        modelBuilder.Entity<Candidato>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Telefone).HasMaxLength(20);
            entity.Property(e => e.Cidade).HasMaxLength(100);
            entity.Property(e => e.Estado).HasMaxLength(2);
            entity.Property(e => e.Formacao).HasMaxLength(200);
            entity.Property(e => e.AreaAtuacao).HasMaxLength(200);
            entity.Property(e => e.ResumoProfissional).HasMaxLength(2000);
            entity.Property(e => e.LinkedIn).HasMaxLength(200);
            entity.Property(e => e.DataCadastro).IsRequired();

            // Relacionamento opcional com Vaga
            entity.HasOne(e => e.Vaga)
                  .WithMany(v => v.Candidatos)
                  .HasForeignKey(e => e.VagaId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed data para desenvolvimento
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var vagas = new List<Vaga>
        {
            new Vaga
            {
                Id = 1,
                Titulo = "Desenvolvedor .NET Senior",
                Descricao = "Vaga para desenvolvedor .NET com experiência em APIs RESTful e arquitetura de microsserviços.",
                Requisitos = "Experiência mínima de 5 anos em .NET, conhecimento em Entity Framework Core, SQL Server, APIs RESTful.",
                Localizacao = "São Paulo - SP",
                SalarioMinimo = 8000,
                SalarioMaximo = 12000,
                TipoContrato = "CLT",
                DataCriacao = DateTime.UtcNow.AddDays(-10),
                Ativa = true
            },
            new Vaga
            {
                Id = 2,
                Titulo = "Analista de RH",
                Descricao = "Vaga para analista de RH com foco em recrutamento e seleção.",
                Requisitos = "Formação em RH ou áreas afins, experiência em recrutamento, conhecimento em sistemas de RH.",
                Localizacao = "Rio de Janeiro - RJ",
                SalarioMinimo = 5000,
                SalarioMaximo = 7000,
                TipoContrato = "CLT",
                DataCriacao = DateTime.UtcNow.AddDays(-5),
                Ativa = true
            }
        };

        var candidatos = new List<Candidato>
        {
            new Candidato
            {
                Id = 1,
                Nome = "João Silva",
                Email = "joao.silva@email.com",
                Telefone = "(11) 98765-4321",
                Cidade = "São Paulo",
                Estado = "SP",
                Formacao = "Ciência da Computação",
                AnosExperiencia = 7,
                AreaAtuacao = "Desenvolvimento .NET",
                ResumoProfissional = "Desenvolvedor com 7 anos de experiência em .NET, especializado em APIs RESTful e arquitetura de microsserviços.",
                LinkedIn = "linkedin.com/in/joaosilva",
                DataCadastro = DateTime.UtcNow.AddDays(-3),
                VagaId = 1
            },
            new Candidato
            {
                Id = 2,
                Nome = "Maria Santos",
                Email = "maria.santos@email.com",
                Telefone = "(21) 98765-4321",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Formacao = "Administração com ênfase em RH",
                AnosExperiencia = 4,
                AreaAtuacao = "Recursos Humanos",
                ResumoProfissional = "Analista de RH com experiência em recrutamento, seleção e gestão de pessoas.",
                LinkedIn = "linkedin.com/in/mariasantos",
                DataCadastro = DateTime.UtcNow.AddDays(-2),
                VagaId = 2
            }
        };

        modelBuilder.Entity<Vaga>().HasData(vagas);
        modelBuilder.Entity<Candidato>().HasData(candidatos);
    }
}

