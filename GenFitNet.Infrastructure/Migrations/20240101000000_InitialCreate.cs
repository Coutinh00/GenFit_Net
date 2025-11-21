using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenFitNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SalarioMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalarioMaximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoContrato = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vagas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Formacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AnosExperiencia = table.Column<int>(type: "int", nullable: false),
                    AreaAtuacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResumoProfissional = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    LinkedIn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VagaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidatos_Vagas_VagaId",
                        column: x => x.VagaId,
                        principalTable: "Vagas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_VagaId",
                table: "Candidatos",
                column: "VagaId");

            // Seed data
            var utcNow = DateTime.UtcNow;
            migrationBuilder.InsertData(
                table: "Vagas",
                columns: new[] { "Id", "Titulo", "Descricao", "Requisitos", "Localizacao", "SalarioMinimo", "SalarioMaximo", "TipoContrato", "DataCriacao", "Ativa" },
                values: new object[,]
                {
                    {
                        1,
                        "Desenvolvedor .NET Senior",
                        "Vaga para desenvolvedor .NET com experiência em APIs RESTful e arquitetura de microsserviços.",
                        "Experiência mínima de 5 anos em .NET, conhecimento em Entity Framework Core, SQL Server, APIs RESTful.",
                        "São Paulo - SP",
                        8000m,
                        12000m,
                        "CLT",
                        utcNow.AddDays(-10),
                        true
                    },
                    {
                        2,
                        "Analista de RH",
                        "Vaga para analista de RH com foco em recrutamento e seleção.",
                        "Formação em RH ou áreas afins, experiência em recrutamento, conhecimento em sistemas de RH.",
                        "Rio de Janeiro - RJ",
                        5000m,
                        7000m,
                        "CLT",
                        utcNow.AddDays(-5),
                        true
                    }
                });

            migrationBuilder.InsertData(
                table: "Candidatos",
                columns: new[] { "Id", "Nome", "Email", "Telefone", "Cidade", "Estado", "Formacao", "AnosExperiencia", "AreaAtuacao", "ResumoProfissional", "LinkedIn", "DataCadastro", "VagaId" },
                values: new object[,]
                {
                    {
                        1,
                        "João Silva",
                        "joao.silva@email.com",
                        "(11) 98765-4321",
                        "São Paulo",
                        "SP",
                        "Ciência da Computação",
                        7,
                        "Desenvolvimento .NET",
                        "Desenvolvedor com 7 anos de experiência em .NET, especializado em APIs RESTful e arquitetura de microsserviços.",
                        "linkedin.com/in/joaosilva",
                        utcNow.AddDays(-3),
                        1
                    },
                    {
                        2,
                        "Maria Santos",
                        "maria.santos@email.com",
                        "(21) 98765-4321",
                        "Rio de Janeiro",
                        "RJ",
                        "Administração com ênfase em RH",
                        4,
                        "Recursos Humanos",
                        "Analista de RH com experiência em recrutamento, seleção e gestão de pessoas.",
                        "linkedin.com/in/mariasantos",
                        utcNow.AddDays(-2),
                        2
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Vagas");
        }
    }
}

