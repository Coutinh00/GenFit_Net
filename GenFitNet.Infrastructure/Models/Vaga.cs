namespace GenFitNet.Infrastructure.Models;

public class Vaga
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Requisitos { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public decimal SalarioMinimo { get; set; }
    public decimal SalarioMaximo { get; set; }
    public string TipoContrato { get; set; } = string.Empty; // CLT, PJ, Estágio, etc.
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativa { get; set; } = true;
    
    // Relacionamento com Candidatos
    public ICollection<Candidato> Candidatos { get; set; } = new List<Candidato>();
}

