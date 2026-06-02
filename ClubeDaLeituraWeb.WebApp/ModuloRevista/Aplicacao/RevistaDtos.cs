namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;

public record CadastrarRevistaDto(
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaId
);

public record EditarRevistaDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaId
);

public record DetalhesRevistaDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaId,
    string CaixaEtiqueta
);
