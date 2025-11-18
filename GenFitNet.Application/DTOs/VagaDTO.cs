namespace GenFitNet.Application.DTOs;

public class VagaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Requisitos { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public decimal SalarioMinimo { get; set; }
    public decimal SalarioMaximo { get; set; }
    public string TipoContrato { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativa { get; set; }
    public int TotalCandidatos { get; set; }
}

public class CreateVagaDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Requisitos { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public decimal SalarioMinimo { get; set; }
    public decimal SalarioMaximo { get; set; }
    public string TipoContrato { get; set; } = string.Empty;
}

public class UpdateVagaDTO
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? Requisitos { get; set; }
    public string? Localizacao { get; set; }
    public decimal? SalarioMinimo { get; set; }
    public decimal? SalarioMaximo { get; set; }
    public string? TipoContrato { get; set; }
    public bool? Ativa { get; set; }
}

