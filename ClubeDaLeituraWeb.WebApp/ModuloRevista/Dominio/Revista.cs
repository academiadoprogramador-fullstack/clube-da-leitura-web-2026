using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

public sealed class Revista : EntidadeBase<Revista>
{
    public string Titulo { get; set; } = string.Empty;
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public Caixa Caixa { get; set; } = null!;
    public StatusRevista Status { get; set; } = StatusRevista.Disponivel;

    public Revista() { }

    public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa, StatusRevista status = StatusRevista.Disponivel)
    {
        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        Caixa = caixa;
        Status = status;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (AnoPublicacao < 1 || AnoPublicacao > DateTime.Now.Year)
            erros.Add("O campo \"Ano de Publicação\" deve conter um ano válido.");

        return erros;
    }

    public override void Atualizar(Revista entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        NumeroEdicao = entidadeAtualizada.NumeroEdicao;
        AnoPublicacao = entidadeAtualizada.AnoPublicacao;
        Caixa = entidadeAtualizada.Caixa;
        Status = entidadeAtualizada.Status;
    }
}
