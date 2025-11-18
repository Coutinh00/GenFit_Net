namespace GenFitNet.Application.DTOs;

public class CandidatoDTO
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
    public int? VagaId { get; set; }
    public string? VagaTitulo { get; set; }
}

public class CreateCandidatoDTO
{
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
    public int? VagaId { get; set; }
}

public class UpdateCandidatoDTO
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Formacao { get; set; }
    public int? AnosExperiencia { get; set; }
    public string? AreaAtuacao { get; set; }
    public string? ResumoProfissional { get; set; }
    public string? LinkedIn { get; set; }
    public int? VagaId { get; set; }
}

public class CandidatoFiltroDTO
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? AreaAtuacao { get; set; }
    public int? AnosExperienciaMinimo { get; set; }
    public int? VagaId { get; set; }
    public string? Formacao { get; set; }
}

