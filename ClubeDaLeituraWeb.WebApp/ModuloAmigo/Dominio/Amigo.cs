using System.Text.RegularExpressions;
using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;

public sealed class Amigo : EntidadeBase<Amigo>
{
    public string Nome { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    public Amigo() { }

    public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = nome;
        NomeResponsavel = nomeResponsavel;
        Telefone = telefone;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O campo \"Nome\" deve ser preenchido.");

        else if (Nome.Length < 3 || Nome.Length > 100)
            erros.Add("O campo \"Nome\" deve conter entre 3 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(NomeResponsavel))
            erros.Add("O campo \"Nome do Responsável\" deve ser preenchido.");

        else if (NomeResponsavel.Length < 3 || NomeResponsavel.Length > 100)
            erros.Add("O campo \"Nome do Responsável\" deve conter entre 3 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(Telefone))
            erros.Add("O campo \"Telefone\" deve ser preenchido.");

        else if (!Regex.IsMatch(Telefone, @"^\d{10,11}$"))
            erros.Add("O campo \"Telefone\" deve conter entre 10 e 11 dígitos.");

        return erros;
    }

    public override void Atualizar(Amigo entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        NomeResponsavel = entidadeAtualizada.NomeResponsavel;
        Telefone = entidadeAtualizada.Telefone;
    }
}
