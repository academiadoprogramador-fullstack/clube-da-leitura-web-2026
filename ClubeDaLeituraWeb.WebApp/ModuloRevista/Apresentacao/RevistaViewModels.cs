using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public record OpcaoCaixaViewModel(
    string Id,
    string Etiqueta
);

public record ListarRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string Caixa,
    string Status
);

public record CadastrarRevistaViewModel(
    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Título\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor maior que 0.")]
    int NumeroEdicao,

    [Range(1, 9999, ErrorMessage = "O campo \"Ano de Publicação\" deve conter um ano válido.")]
    int AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId,

    [ValidateNever]
    List<OpcaoCaixaViewModel> Caixas
);

public record EditarRevistaViewModel(
    string Id,

    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Título\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor maior que 0.")]
    int NumeroEdicao,

    [Range(1, 9999, ErrorMessage = "O campo \"Ano de Publicação\" deve conter um ano válido.")]
    int AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId,

    [ValidateNever]
    List<OpcaoCaixaViewModel> Caixas
);

public record ExcluirRevistaViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaEtiqueta
);
