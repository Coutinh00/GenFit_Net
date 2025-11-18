namespace GenFitNet.Infrastructure.Models;

public class Candidato
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Formacao { get; set; } = string.Empty;
    public int AnosExperiencia { get; set; }
    public string AreaAtuacao { get; set; } = string.Empty;
    public string ResumoProfissional { get; set; } = string.Empty;
    public string LinkedIn { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    
    // Relacionamento com Vagas
    public int? VagaId { get; set; }
    public Vaga? Vaga { get; set; }
}

