namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;

public record CadastrarRevistaDto(
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string CaixaId
);
