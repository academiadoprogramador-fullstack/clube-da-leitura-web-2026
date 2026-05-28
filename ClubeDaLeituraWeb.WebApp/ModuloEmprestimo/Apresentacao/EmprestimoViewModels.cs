using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Apresentacao;

public record OpcaoAmigoViewModel(
    string Id,
    string Nome
);

public record OpcaoRevistaViewModel(
    string Id,
    string Titulo
);

public record ListarEmprestimosViewModel(
    string Id,
    string Amigo,
    string Revista,
    DateTime DataEmprestimo,
    DateTime DataDevolucao,
    DateTime? DataDevolvido,
    string Status,
    bool EstaAtrasado
);

public record CadastrarEmprestimoViewModel(
    [Required(ErrorMessage = "O campo \"Amigo\" deve ser preenchido.")]
    string AmigoId,

    [Required(ErrorMessage = "O campo \"Revista\" deve ser preenchido.")]
    string RevistaId,

    [ValidateNever]
    List<OpcaoAmigoViewModel> Amigos,

    [ValidateNever]
    List<OpcaoRevistaViewModel> Revistas
);

public record DevolverEmprestimoViewModel(
    string Id,
    string Amigo,
    string Revista,
    DateTime DataEmprestimo,
    DateTime DataDevolucao
);
