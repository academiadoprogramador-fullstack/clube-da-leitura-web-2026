using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;

public sealed class Emprestimo : EntidadeBase<Emprestimo>
{
    public Amigo Amigo { get; set; } = null!;
    public Revista Revista { get; set; } = null!;
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; }
    public DateTime? DataDevolvido { get; set; }

    public StatusEmprestimo Status
    {
        get
        {
            if (DataDevolvido.HasValue)
                return StatusEmprestimo.Concluido;

            if (DateTime.Today > DataDevolucao.Date)
                return StatusEmprestimo.Atrasado;

            return StatusEmprestimo.Aberto;
        }
    }

    public Emprestimo() { }

    public Emprestimo(Amigo amigo, Revista revista, DateTime dataEmprestimo, DateTime dataDevolucao)
    {
        Amigo = amigo;
        Revista = revista;
        DataEmprestimo = dataEmprestimo;
        DataDevolucao = dataDevolucao;
    }

    public void RegistrarDevolucao()
    {
        DataDevolvido = DateTime.Today;
        Revista.Status = StatusRevista.Disponivel;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (Amigo == null)
            erros.Add("O campo \"Amigo\" deve ser preenchido.");

        if (Revista == null)
            erros.Add("O campo \"Revista\" deve ser preenchido.");

        if (DataEmprestimo == default)
            erros.Add("O campo \"Data de Empréstimo\" deve ser preenchido.");

        if (DataDevolucao == default)
            erros.Add("O campo \"Data de Devolução\" deve ser preenchido.");

        return erros;
    }

    public override void Atualizar(Emprestimo entidadeAtualizada)
    {
        Amigo = entidadeAtualizada.Amigo;
        Revista = entidadeAtualizada.Revista;
        DataEmprestimo = entidadeAtualizada.DataEmprestimo;
        DataDevolucao = entidadeAtualizada.DataDevolucao;
        DataDevolvido = entidadeAtualizada.DataDevolvido;
    }
}
