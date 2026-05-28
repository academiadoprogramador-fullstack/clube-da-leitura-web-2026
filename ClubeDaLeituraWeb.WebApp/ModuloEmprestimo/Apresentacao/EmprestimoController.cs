using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Apresentacao;

public class EmprestimoController : Controller
{
    private readonly IRepositorioEmprestimo repositorioEmprestimo;
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioRevista repositorioRevista;

    public EmprestimoController(
        IRepositorioEmprestimo repositorioEmprestimo,
        IRepositorioAmigo repositorioAmigo,
        IRepositorioRevista repositorioRevista
    )
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Emprestimo> emprestimos = repositorioEmprestimo.SelecionarTodos();

        return View(MapearEmprestimos(emprestimos));
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarEmprestimoViewModel cadastrarVm = new CadastrarEmprestimoViewModel(
            string.Empty,
            string.Empty,
            SelecionarAmigos(),
            SelecionarRevistasDisponiveis()
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarEmprestimoViewModel cadastrarVm)
    {
        Amigo? amigoSelecionado = repositorioAmigo.SelecionarPorId(cadastrarVm.AmigoId);
        Revista? revistaSelecionada = repositorioRevista.SelecionarPorId(cadastrarVm.RevistaId);

        if (amigoSelecionado == null)
            ModelState.AddModelError(nameof(cadastrarVm.AmigoId), "Selecione um amigo válido.");

        if (revistaSelecionada == null)
            ModelState.AddModelError(nameof(cadastrarVm.RevistaId), "Selecione uma revista válida.");

        else if (revistaSelecionada.Status != StatusRevista.Disponivel)
            ModelState.AddModelError(nameof(cadastrarVm.RevistaId), "Selecione uma revista disponível.");

        if (!ModelState.IsValid)
            return View(cadastrarVm with
            {
                Amigos = SelecionarAmigos(),
                Revistas = SelecionarRevistasDisponiveis()
            });

        DateTime dataEmprestimo = DateTime.Today;
        DateTime dataDevolucao = dataEmprestimo.AddDays(revistaSelecionada!.Caixa.DiasDeEmprestimo);

        Emprestimo novoEmprestimo = new Emprestimo(
            amigoSelecionado!,
            revistaSelecionada,
            dataEmprestimo,
            dataDevolucao
        );

        revistaSelecionada.Status = StatusRevista.Emprestada;

        repositorioEmprestimo.Cadastrar(novoEmprestimo);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Devolver(string id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimo == null || emprestimo.DataDevolvido.HasValue)
            return RedirectToAction(nameof(Listar));

        DevolverEmprestimoViewModel devolverVm = new DevolverEmprestimoViewModel(
            id,
            emprestimo.Amigo.Nome,
            emprestimo.Revista.Titulo,
            emprestimo.DataEmprestimo,
            emprestimo.DataDevolucao
        );

        return View(devolverVm);
    }

    [HttpPost]
    public ActionResult Devolver(DevolverEmprestimoViewModel devolverVm)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(devolverVm.Id);

        if (emprestimo != null && !emprestimo.DataDevolvido.HasValue)
        {
            emprestimo.RegistrarDevolucao();

            repositorioEmprestimo.Editar(emprestimo.Id, emprestimo);
        }

        return RedirectToAction(nameof(Listar));
    }

    private List<OpcaoAmigoViewModel> SelecionarAmigos()
    {
        return repositorioAmigo.SelecionarTodos()
            .Select(a => new OpcaoAmigoViewModel(a.Id, a.Nome))
            .ToList();
    }

    private List<OpcaoRevistaViewModel> SelecionarRevistasDisponiveis()
    {
        return repositorioRevista.Filtrar(r => r.Status == StatusRevista.Disponivel)
            .Select(r => new OpcaoRevistaViewModel(r.Id, $"{r.Titulo} #{r.NumeroEdicao}"))
            .ToList();
    }

    private List<ListarEmprestimosViewModel> MapearEmprestimos(List<Emprestimo> emprestimos)
    {
        List<ListarEmprestimosViewModel> listarVms = emprestimos.Select(e => new ListarEmprestimosViewModel(
            e.Id,
            e.Amigo.Nome,
            e.Revista.Titulo,
            e.DataEmprestimo,
            e.DataDevolucao,
            e.DataDevolvido,
            FormatarStatus(e.Status),
            e.Status == StatusEmprestimo.Atrasado
        )).ToList();

        return listarVms;
    }

    private string FormatarStatus(StatusEmprestimo status)
    {
        switch (status)
        {
            case StatusEmprestimo.Concluido: return "Concluído";
            case StatusEmprestimo.Atrasado: return "Atrasado";
            default: return "Aberto";
        }
    }
}
